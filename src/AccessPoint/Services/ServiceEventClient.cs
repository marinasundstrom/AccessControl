using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccessControl.Messages.Events;
using Microsoft.Azure.Devices.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AccessPoint.Services
{
    public sealed class ServiceEventClient : IServiceEventClient
    {
        private readonly ILogger<ServiceEventClient> _logger;
        private DeviceClient _deviceClient;

        public ServiceEventClient(
           ILogger<ServiceEventClient> logger,
           DeviceClient deviceClient)
        {
            _logger = logger;
            _deviceClient = deviceClient;
        }

        public async Task SendEventAsync(Event ev)
        {
            var data = JsonConvert.SerializeObject(ev);
            await _deviceClient.SendEventAsync(new Message(Encoding.UTF8.GetBytes(data)));
        }
    }
}
