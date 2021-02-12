using AccessControl.Messages.Commands;
using AccessControl.Messages.Events;
using AccessPoint.Components;
using AccessPoint.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Devices.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AccessEvent = AccessControl.Messages.Events.AccessEvent;

namespace AccessPoint.Services
{
    public sealed class AccessPointService : IAccessPointService
    {
        private readonly ILogger<AccessPointService> logger;
        private readonly AccessPointContext _accessPointContext;
        private readonly ISwitchService _switchService;
        private readonly ILEDService _ledService;

        private IDisposable whenSwitchClosedSubscription;
        private IDisposable whenSwitchOpenedSubscription;
        private IDisposable whenCardDataReceivedSubscription;

        private readonly IBuzzerService _buzzerService;
        private readonly IRelayControlService _relayControlService;
        private readonly ICommandReceiver _commandReceiver;
        private readonly IServiceEventClient _serviceEventClient;
        private readonly IRfidReader _rfidReader;

        private readonly AppService.Contracts.IAuthorizationClient _authorizationClient;

        private const int RedLED = 0;
        private const int GreenLED = 1;
        private const int BlueLED = 2;
        private const int LockRelay = 0;

        private bool _lockWhenShut = true;
        private bool _armWhenShut = true;

        private TimeSpan _accessTime = TimeSpan.FromSeconds(10); // 30 seconds
        private TimeSpan _buzzTime = TimeSpan.FromSeconds(5);

        private Timer _timer = null;

        private bool authenticated;
        private bool _unlocked;

        public AccessPointService(
            ILogger<AccessPointService> logger,
            AccessPointContext accessPointContext,
            ISwitchService switchService,
            ILEDService ledService,
            IBuzzerService buzzerService,
            IRelayControlService relayControlService,
            ICommandReceiver commandReceiver,
            IServiceEventClient serviceEventClient,
            IRfidReader rfidReader,
            AppService.Contracts.IAuthorizationClient authorizationClient)
        {
            this.logger = logger;

            _accessPointContext = accessPointContext;
            _switchService = switchService;
            _ledService = ledService;
            _buzzerService = buzzerService;
            _relayControlService = relayControlService;
            _commandReceiver = commandReceiver;
            _serviceEventClient = serviceEventClient;
            _rfidReader = rfidReader;
            _authorizationClient = authorizationClient;

            WhenSwitchOpened = Observable.FromEventPattern(
                handler => _switchService.Opened += handler,
                handler => _switchService.Opened -= handler);

            WhenSwitchClosed = Observable.FromEventPattern(
                handler => _switchService.Closed += handler,
                handler => _switchService.Closed -= handler);
        }

        private IObservable<EventPattern<object>> WhenSwitchOpened { get; }
        private IObservable<EventPattern<object>> WhenSwitchClosed { get; }

        public void Dispose()
        {

        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _accessTime = TimeSpan.Parse(await GetParam("accessTime") ?? "00:00:10");
            _lockWhenShut = bool.Parse(await GetParam("lockOnClose") ?? bool.TrueString);
            _armWhenShut = bool.Parse(await GetParam("armOnClose") ?? bool.TrueString);

            await _relayControlService.SetRelayStateAsync(1, false);

            await _commandReceiver.SetCommandHandler<Command, object>(CommandHandler);

            whenSwitchClosedSubscription = WhenSwitchClosed.Subscribe(async _ =>
                await OnDoorClosed());

            whenSwitchOpenedSubscription = WhenSwitchOpened.Subscribe(async _ =>
                await OnDoorOpened());

            whenCardDataReceivedSubscription = _rfidReader
                .WhenCardDetected
                .Throttle(TimeSpan.FromMilliseconds(800))
                .Subscribe(async cardData => await OnCardRead(cardData));

            await _rfidReader.StartAsync();
        }

        private async Task OnCardRead(CardData cardData)
        {
            // Console.WriteLine(string.Join(", ", cardData.UID));

            var ct = new CancellationTokenSource();

            try
            {
                _ = BlinkBlue(ct.Token);

                var result = await _authorizationClient.AuthorizeAsync(new AppService.Contracts.AuthorizeCardCommand()
                {
                    DeviceId = "AccessPoint1",
                    CardNo = cardData.UID,
                    Pin = null
                });

                ct.Cancel();

                if (result.Authorized)
                {
                    await OnAccessRequested();
                }
                else
                {
                    await ToggleRedLedOn();

                    await Task.Delay(3000);

                    await ToggleAllLedsOff();
                }
            }
            catch
            {
                ct.Cancel();
            }
        }

        private async Task BlinkBlue(CancellationToken cancellationToken)
        {
            await Task.Delay(1500);

            for (int i = 0; i < 5; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                await ToggleBlueLedOn();

                await _buzzerService.BuzzAsync(TimeSpan.FromSeconds(1));

                await ToggleAllLedsOff();

                await Task.Delay(TimeSpan.FromSeconds(1));

                if (cancellationToken.IsCancellationRequested)
                    return;
            }
        }

        private async Task<object> CommandHandler(Command command)
        {
            switch (command.CommandName)
            {
                case ArmCommand.ArmCommandConstant:
                    await Arm();
                    return GetAlarmStatus();

                case DisarmCommand.DisarmCommandConstant:
                    await OnAccessRequested();
                    return GetAlarmStatus(); 

                case ConfigureCommand.ConfigureCommandConstant:
                    //Console.WriteLine(JsonConvert.SerializeObject(command.Args));
                    _accessTime = TimeSpan.Parse((string)command.Args["accessTime"]);
                    _lockWhenShut = (bool)command.Args["lockOnClose"];
                    _armWhenShut = (bool)command.Args["armOnClose"];
                    await SetParam("accessTime", _accessTime.ToString());
                    await SetParam("lockOnClose", _lockWhenShut.ToString());
                    await SetParam("armOnClose", _armWhenShut.ToString());
                    break;

                case GetConfigurationCommand.GetConfigurationCommandConstant:
                    return new GetConfigurationResponse(_accessTime, _lockWhenShut, _armWhenShut);

                case GetAlarmStateCommand.GetAlarmStateCommandConstant:
                    return GetAlarmStatus();
            }

            return string.Empty;
        }

        private object GetAlarmStatus()
        {
            return new GetAlarmStateResponse(authenticated ?
                 AccessControl.Messages.Commands.AlarmState.Armed
                 : AccessControl.Messages.Commands.AlarmState.Disarmed);
        }

        private async Task<string> GetParam(string key)
        {
            var param = await _accessPointContext.FindAsync<Parameter>(key);
            return param?.Value;
        }

        private async Task SetParam(string key, string value)
        {
            var param = await _accessPointContext.FindAsync<Parameter>(key);
            if (param == null)
            {
                _accessPointContext.Settings.Add(new Parameter()
                {
                    Key = key,
                    Value = value
                });
            }
            else
            {
                param.Value = value;
                _accessPointContext.Update(param);
            }
            await _accessPointContext.SaveChangesAsync();
        }

        private async Task Arm()
        {
            authenticated = false;
            _unlocked = false;

            await _relayControlService.SetRelayStateAsync(LockRelay, false);

            await _serviceEventClient.SendEventAsync(new LockEvent(LockState.Locked));
            await _serviceEventClient.SendEventAsync(new AlarmEvent(AccessControl.Messages.Events.AlarmState.Armed));

            await ToggleAllLedsOff();
        }

        private async Task OnDoorOpened()
        {
            Console.WriteLine(_unlocked);

            if (!_unlocked) //_cardOK
            {
                _unlocked = false;

                await _serviceEventClient.SendEventAsync(new UnauthorizedAccessEvent());

                await ToggleRedLedOn();

                await _buzzerService
                    .BuzzAsync(_buzzTime)
                    .ConfigureAwait(false);

                await ToggleAllLedsOff();

                return;
            }
    
            await _serviceEventClient.SendEventAsync(new AccessEvent());
        }

        private async Task OnDoorClosed()
        {
            if (authenticated)
            {
                if (_lockWhenShut)
                {
                    await _serviceEventClient.SendEventAsync(new LockEvent(LockState.Locked));
                    await _relayControlService.SetRelayStateAsync(LockRelay, false);

                    _unlocked = false;

                    await ToggleAllLedsOff();
                }

                if(_armWhenShut)
                {
                    await _serviceEventClient.SendEventAsync(new AlarmEvent(AccessControl.Messages.Events.AlarmState.Armed));

                    await ToggleAllLedsOff();
                }

                _timer?.Dispose();
                authenticated = false;
            }
        }

        private async Task OnAccessRequested()
        {
            try
            {
                //if (_unlocked) return;

                authenticated = true; //await CheckCardAsync(cardData);

                if (authenticated)
                {
                    _unlocked = true;

                    await _serviceEventClient.SendEventAsync(new AlarmEvent(AccessControl.Messages.Events.AlarmState.Disarmed));
                    await _serviceEventClient.SendEventAsync(new LockEvent(LockState.Unlocked));

                    await ToggleGreenLedOn();

                    await _relayControlService.SetRelayStateAsync(LockRelay, true);

                    if (_accessTime != TimeSpan.Zero) // Infinite access time
                    {
                        _timer = new Timer(async _ =>
                        {
                            _timer?.Dispose();
                            _timer = null;
                            authenticated = false;
                            _unlocked = false;

                            await _relayControlService.SetRelayStateAsync(LockRelay, false);

                            await _serviceEventClient.SendEventAsync(new LockEvent(LockState.Locked));
                            await _serviceEventClient.SendEventAsync(new AlarmEvent(AccessControl.Messages.Events.AlarmState.Armed));

                            await ToggleAllLedsOff();
                        }, null, (int)_accessTime.TotalMilliseconds, 0);
                    }
                }
                //else
                //{
                //    //await ToggleRedLedOn();

                //    //await _relayControlService.SetRelayStateAsync(LockRelay, false);

                //    //_timer = new Timer(async _ =>
                //    //{
                //    //    _timer?.Dispose();
                //    //    _timer = null;
                //    //    _cardOK = false;
                //    //    _unlocked = false;
                //    //    await ToggleAllLedsOff();
                //    //}, null, (int)_buzzTime.TotalMilliseconds, 0);
                //}
            }
            catch (Exception e)
            {
                logger.LogError(e, string.Empty);
            }
        }

        private async Task ToggleAllLedsOff()
        {
            await _ledService.SetAsync(GreenLED, false);
            await _ledService.SetAsync(RedLED, false);
            await _ledService.SetAsync(BlueLED, false);
        }

        private async Task ToggleRedLedOn()
        {
            await _ledService.SetAsync(RedLED, true);
            await _ledService.SetAsync(GreenLED, false);
            await _ledService.SetAsync(BlueLED, false);
        }

        private async Task ToggleGreenLedOn()
        {
            await _ledService.SetAsync(RedLED, false);
            await _ledService.SetAsync(GreenLED, true);
            await _ledService.SetAsync(BlueLED, false);
        }

        private async Task ToggleBlueLedOn()
        {
            await _ledService.SetAsync(RedLED, false);
            await _ledService.SetAsync(GreenLED, false);
            await _ledService.SetAsync(BlueLED, true);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            whenSwitchClosedSubscription.Dispose();
            whenSwitchOpenedSubscription.Dispose();

            await _rfidReader.StopAsync();

            whenCardDataReceivedSubscription.Dispose();
        }
    }
}
