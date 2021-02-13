using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AccessPoint.Services
{
    public class CommandReceiver : ICommandReceiver
    {
        private const string MethodName = "Command";

        private readonly ILogger<CommandReceiver> _logger;
        private DeviceClient _deviceClient;

        public CommandReceiver(
           ILogger<CommandReceiver> logger,
           DeviceClient deviceClient)
        {
            _logger = logger;
            _deviceClient = deviceClient;
        }

        public async Task SetCommandHandler<A, R>(Func<A, Task<R>> handler)
        {
            await _deviceClient.SetMethodHandlerAsync(MethodName, async (MethodRequest methodRequest, object userContext) =>
            {
                var arg = (A)JsonConvert.DeserializeObject(methodRequest.DataAsJson, typeof(A));
                try
                {
                    var result = await handler(arg);
                    var json = JsonConvert.SerializeObject(result);
                    return new MethodResponse(Encoding.UTF8.GetBytes(json), 200);
                }
                catch
                {
                    return new MethodResponse(500);
                }
            }, null);
        }
    }
}
