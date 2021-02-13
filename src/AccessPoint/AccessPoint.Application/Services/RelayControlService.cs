using AccessPoint.Application.Components;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Threading.Tasks;

namespace AccessPoint.Application.Services
{
    public class RelayControlService : IRelayControlService
    {
        private Relay[] relays;

        public RelayControlService(GpioController gpioController)
        {
            relays = new Relay[] {
                new Relay(gpioController, 20),
                new Relay(gpioController, 16)
            };
        }

        public void Dispose()
        {
            foreach(var relay in relays)
            {
                relay.Dispose();
            }
        }

        public Task<IEnumerable<RelayInfo>> GetRelaysAsync() => Task.FromResult(relays.Select(x => new RelayInfo() { State = x.GetRelayState() }));

        public Task<bool> GetRelayStateAsync(int relay) => Task.FromResult(relays[relay].GetRelayState());

        public Task SetRelayStateAsync(int relay, bool state) => Task.Run(() => relays[relay].SetRelayState(state));
    }
}
