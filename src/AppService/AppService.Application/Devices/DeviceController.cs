using AccessControl.Messages.Commands;
using Microsoft.Azure.Devices;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.NotificationHubs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppService.Application.Devices
{
    public class DeviceController
    {
        private readonly ServiceClient serviceClient;

        public DeviceController(
            ServiceClient serviceClient)
        {
            this.serviceClient = serviceClient;
        }

        public async Task<GetAlarmStateResponse> GetState(string deviceId)
        {
            var response = await SendCommand<GetAlarmStateResponse>(deviceId, new GetAlarmStateCommand());
            return response;
        }

        public async Task Arm(string deviceId)
        {
            await SendCommand(deviceId, new ArmCommand());
        }

        public async Task Disarm(string deviceId)
        {
            await SendCommand(deviceId, new DisarmCommand());
        }

        public async Task Configure(string deviceId, TimeSpan accessTime, bool lockOnClose, bool armOnClose)
        {
            await SendCommand(deviceId, new ConfigureCommand(accessTime, lockOnClose, armOnClose));
        }

        public async Task<GetConfigurationResponse> GetConfiguration(string deviceId)
        {
            return await SendCommand<GetConfigurationResponse>(deviceId, new GetConfigurationCommand());
        }

        public async Task<TagData> ReadRfidTag(string deviceId)
        {
            return await SendCommand<TagData>(deviceId, new ReadTagCommand());
        }

        private async Task<T> SendCommand<T>(string deviceId, Command command)
        {
            var response = await SendCommand(deviceId, command);
            return (T)JsonConvert.DeserializeObject(response.GetPayloadAsJson(), typeof(T));
        }

        private async Task<CloudToDeviceMethodResult> SendCommand(string deviceId, Command command)
        {
            var deviceMethod = new CloudToDeviceMethod("Command");
            var json = JsonConvert.SerializeObject(command);
            deviceMethod.SetPayloadJson(json);
            var response = await serviceClient.InvokeDeviceMethodAsync(deviceId, deviceMethod);
            return response;
        }
    }
}
