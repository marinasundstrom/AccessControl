using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccessControl.Contracts.Commands;
using Microsoft.Azure.NotificationHubs;
using Newtonsoft.Json;
using MassTransit;

namespace AppService.Application.Devices
{
    public class DeviceController
    {
        private readonly IRequestClient<GetAlarmStateCommand> _getAlarmStateCommandClient;
        private readonly IRequestClient<ArmCommand> _armCommandClient;
        private readonly IRequestClient<DisarmCommand> _disarmCommandClient;
        private readonly IRequestClient<ConfigureCommand> _configureCommandClient;
        private readonly IRequestClient<GetConfigurationCommand> _getConfigureCommandClient;
        private readonly IRequestClient<ReadTagCommand> _readTagCommandClient;

        public DeviceController(
            IRequestClient<GetAlarmStateCommand> getAlarmStateCommandClient,
            IRequestClient<ArmCommand> armCommandClient,
            IRequestClient<DisarmCommand> disarmCommandClient,
            IRequestClient<ConfigureCommand> configureCommandClient,
            IRequestClient<GetConfigurationCommand> getConfigureCommandClient,
            IRequestClient<ReadTagCommand> readTagCommandClient)
        {
            _getAlarmStateCommandClient = getAlarmStateCommandClient;
            _armCommandClient = armCommandClient;
            _disarmCommandClient = disarmCommandClient;
            _configureCommandClient = configureCommandClient;
            _getConfigureCommandClient = getConfigureCommandClient;
            _readTagCommandClient = readTagCommandClient;
        }

        public async Task<GetAlarmStateCommandResponse> GetState(string deviceId)
        {
            var response = await _getAlarmStateCommandClient.GetResponse<GetAlarmStateCommandResponse>(new GetAlarmStateCommand());
            return response.Message;
        }

        public async Task Arm(string deviceId)
        {
            var response = await _armCommandClient.GetResponse<ArmCommandResponse>(new ArmCommand());
        }

        public async Task Disarm(string deviceId)
        {
            var response = await _disarmCommandClient.GetResponse<DisarmCommandResponse>(new DisarmCommand());
        }

        public async Task Configure(string deviceId, TimeSpan accessTime, bool lockOnClose, bool armOnClose)
        {
            var response = await _configureCommandClient.GetResponse<ConfigureCommandResponse>(new ConfigureCommand(accessTime, lockOnClose, armOnClose));
        }

        public async Task<GetConfigurationCommandResponse> GetConfiguration(string deviceId)
        {
            var response = await _getConfigureCommandClient.GetResponse<GetConfigurationCommandResponse>(new GetConfigurationCommand());
            return response.Message;
        }

        public async Task<ReadTagCommandResponse> ReadRfidTag(string deviceId)
        {
            var response = await _readTagCommandClient.GetResponse<ReadTagCommandResponse>(new ReadTagCommand());
            return response.Message;
        }
    }
}
