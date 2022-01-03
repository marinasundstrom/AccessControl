using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccessControl.Contracts.Commands;
using AccessPoint.Application.Models;
using AccessPoint.Application.Rfid.Notifications;
using AccessPoint.Application.Sensors.Notifications;
using AppService;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AccessPoint.Application.Services
{

    public sealed class AccessPointService : IAccessPointService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMediator _mediator;
        private readonly IButtonService _buttonService;

        private readonly ISwitchService _switchService;
        private readonly IRfidReader _rfidReader;
        private readonly IRelayControlService _relayControlService;
        private readonly IPirSensorService _pirSensorService;

        private readonly AccessPointState _state;

        private IDisposable whenSwitchClosedSubscription;
        private IDisposable whenSwitchOpenedSubscription;
        private IDisposable whenMotionDetectedSubscription;
        private IDisposable whenMotionNotDetectedSubscription;
        private IDisposable whenCardDataReceivedSubscription;
        private IDisposable whenButtonPressedSubscription;
        private IDisposable whenButtonReleasedSubscription;

        public AccessPointService(
            IServiceProvider ServiceProvider,
            IMediator mediator,
            IButtonService buttonService,
            ISwitchService switchService,
            IRfidReader rfidReader,
            IRelayControlService relayControlService,
            IPirSensorService pirSensorService,
            AccessPointState state,
            IServiceEventClient serviceEventClient)
        {
            _serviceProvider = ServiceProvider;
            _mediator = mediator;
            _buttonService = buttonService;
            _switchService = switchService;
            _rfidReader = rfidReader;
            _relayControlService = relayControlService;
            _pirSensorService = pirSensorService;
            _state = state;

            WhenButtonPressedOpened = Observable.FromEventPattern(
                handler => _buttonService.Pressed += handler,
                handler => _buttonService.Pressed -= handler);

            WhenButtonReleasedClosed = Observable.FromEventPattern(
                handler => _buttonService.Released += handler,
                handler => _buttonService.Released -= handler);

            WhenSwitchOpened = Observable.FromEventPattern(
                handler => _switchService.Opened += handler,
                handler => _switchService.Opened -= handler);

            WhenSwitchClosed = Observable.FromEventPattern(
                handler => _switchService.Closed += handler,
                handler => _switchService.Closed -= handler);

            WhenMotionDetected = Observable.FromEventPattern(
              handler => _pirSensorService.MotionDetected += handler,
              handler => _pirSensorService.MotionDetected -= handler);

            WhenMotionNotDetected = Observable.FromEventPattern(
              handler => _pirSensorService.MotionNotDetected += handler,
              handler => _pirSensorService.MotionNotDetected -= handler);
        }


        public IObservable<EventPattern<object>> WhenButtonPressedOpened { get; }
        public IObservable<EventPattern<object>> WhenButtonReleasedClosed { get; }

        private IObservable<EventPattern<object>> WhenSwitchOpened { get; }
        private IObservable<EventPattern<object>> WhenSwitchClosed { get; }

        private IObservable<EventPattern<object>> WhenMotionDetected { get; }
        private IObservable<EventPattern<object>> WhenMotionNotDetected { get; }

        public void Dispose()
        {

        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _state.AccessTime = TimeSpan.Parse(await GetParam("accessTime") ?? "00:00:10");
            _state.LockWhenShut = bool.Parse(await GetParam("lockOnClose") ?? bool.TrueString);
            _state.ArmWhenShut = bool.Parse(await GetParam("armOnClose") ?? bool.TrueString);

            await _relayControlService.SetRelayStateAsync(1, false);

            whenButtonPressedSubscription = WhenButtonPressedOpened.Subscribe(async _ =>
                await _mediator.Publish(new ButtonPressedNotification()));

            whenButtonReleasedSubscription = WhenButtonReleasedClosed.Subscribe(async _ =>
                await _mediator.Publish(new ButtonReleasedNotification()));

            whenSwitchClosedSubscription = WhenSwitchClosed.Subscribe(async _ =>
                await _mediator.Publish(new DoorClosedNotification()));

            whenSwitchOpenedSubscription = WhenSwitchOpened.Subscribe(async _ =>
                await _mediator.Publish(new DoorOpenedNotification()));

            whenMotionDetectedSubscription = WhenMotionDetected.Subscribe(async _ =>
              await _mediator.Publish(new MotionDetectedNotification()));

            whenMotionNotDetectedSubscription = WhenMotionNotDetected.Subscribe(async _ =>
              await _mediator.Publish(new MotionNotDetectedNotification()));

            whenCardDataReceivedSubscription = _rfidReader
                .WhenCardDetected
                .Throttle(TimeSpan.FromMilliseconds(800))
                .Subscribe(async cardData =>
                await _mediator.Publish(new OnTagReadNotification(cardData)));

            await _rfidReader.StartAsync();
        }

        private object GetAlarmStatus()
        {
            return new GetAlarmStateCommandResponse(_state.Armed ?
                 AccessControl.Contracts.Commands.AlarmState.Armed
                 : AccessControl.Contracts.Commands.AlarmState.Disarmed);
        }

        private async Task<string> GetParam(string key)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var param = await scope.ServiceProvider
                    .GetRequiredService<AccessPointContext>()
                    .FindAsync<Parameter>(key);

                return param?.Value;
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            whenButtonPressedSubscription.Dispose();
            whenButtonReleasedSubscription.Dispose();
            
            whenSwitchClosedSubscription.Dispose();
            whenSwitchOpenedSubscription.Dispose();

            whenMotionDetectedSubscription.Dispose();
            whenMotionNotDetectedSubscription.Dispose();

            await _rfidReader.StopAsync();

            whenCardDataReceivedSubscription.Dispose();
        }
    }
}
