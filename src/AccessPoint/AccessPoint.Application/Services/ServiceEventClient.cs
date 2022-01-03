using System.Text;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace AccessPoint.Application.Services
{
    public sealed class ServiceEventClient : IServiceEventClient
    {
        private readonly IBus _serviceBus;

        public ServiceEventClient(
           IBus serviceBus)
        {
            _serviceBus = serviceBus;
        }

        public async Task PublishEvent(object ev)
        {
            await _serviceBus.Publish(ev);
        }
    }
}
