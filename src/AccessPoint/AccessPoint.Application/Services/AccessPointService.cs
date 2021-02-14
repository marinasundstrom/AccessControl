using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccessControl.Messages.Commands;
using AccessPoint.Application.Authorization.Notifications;
using AccessPoint.Application.Models;
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

        private readonly ICommandReceiver _commandReceiver;
        private readonly IServiceEventClient _serviceEventClient;

        private readonly ISwitchService _switchService;
        private readonly IRfidReader _rfidReader;
        private readonly IRelayControlService _relayControlService;
        private readonly AccessPointState _state;

        private IDisposable whenSwitchClosedSubscription;
        private IDisposable whenSwitchOpenedSubscription;
        private IDisposable whenCardDataReceivedSubscription;

        public AccessPointService(
            IServiceProvider ServiceProvider,
            IMediator mediator,
            ISwitchService switchService,
            IRfidReader rfidReader,
            IRelayControlService relayControlService,
            AccessPointState state,
            ICommandReceiver commandReceiver,
            IServiceEventClient serviceEventClient)
        {
            _serviceProvider = ServiceProvider;
            _mediator = mediator;
            _switchService = switchService;
            _rfidReader = rfidReader;
            _relayControlService = relayControlService;
            _state = state;
            _commandReceiver = commandReceiver;
            _serviceEventClient = serviceEventClient;

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
            _state.AccessTime = TimeSpan.Parse(await GetParam("accessTime") ?? "00:00:10");
            _state.LockWhenShut = bool.Parse(await GetParam("lockOnClose") ?? bool.TrueString);
            _state.ArmWhenShut = bool.Parse(await GetParam("armOnClose") ?? bool.TrueString);

            await _relayControlService.SetRelayStateAsync(1, false);

            await _commandReceiver.SetCommandHandler<Command, object>(CommandHandler);

            whenSwitchClosedSubscription = WhenSwitchClosed.Subscribe(async _ =>
                await _mediator.Publish(new DoorClosedNotification()));

            whenSwitchOpenedSubscription = WhenSwitchOpened.Subscribe(async _ =>
                await _mediator.Publish(new DoorOpenedNotification()));

            whenCardDataReceivedSubscription = _rfidReader
                .WhenCardDetected
                .Throttle(TimeSpan.FromMilliseconds(800))
                .Subscribe(async cardData =>
                await _mediator.Publish(new CardReadNotification(cardData)));

            await _rfidReader.StartAsync();
        }

        private async Task<object> CommandHandler(Command command)
        {
            switch (command.CommandName)
            {
                case AccessControl.Messages.Commands.ArmCommand.ArmCommandConstant:
                    var result = await _mediator.Send(new Alarm.Commands.ArmCommand());
                    return GetAlarmStatus();

                case AccessControl.Messages.Commands.DisarmCommand.DisarmCommandConstant:
                    var result2 = await _mediator.Send(new Alarm.Commands.DisarmCommand());
                    return GetAlarmStatus(); 

                case ConfigureCommand.ConfigureCommandConstant:
                    var result3 = await _mediator.Send(new Configuration.Commands.SetConfigurationCommand(command.Args));
                    break;

                case GetConfigurationCommand.GetConfigurationCommandConstant:
                    var result4 = await _mediator.Send(new Configuration.Queries.GetConfigurationQuery());
                    return new GetConfigurationResponse(_state.AccessTime, _state.LockWhenShut, _state.ArmWhenShut);

                case GetAlarmStateCommand.GetAlarmStateCommandConstant:
                    var result5 = await _mediator.Send(new Alarm.Queries.GetAlarmStateQuery());
                    return GetAlarmStatus();
            }

            return string.Empty;
        }

        private object GetAlarmStatus()
        {
            return new GetAlarmStateResponse(_state.Authenticated ?
                 AccessControl.Messages.Commands.AlarmState.Armed
                 : AccessControl.Messages.Commands.AlarmState.Disarmed);
        }

        private async Task<string> GetParam(string key)
        {
            using(var scope = _serviceProvider.CreateScope())
            {
                var param = await scope.ServiceProvider
                    .GetRequiredService<AccessPointContext>()
                    .FindAsync<Parameter>(key);

                return param?.Value;
            }
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
