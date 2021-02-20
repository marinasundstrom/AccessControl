using System;
using System.Device.Gpio;
using System.Threading;
using System.Threading.Tasks;

namespace AccessPoint.Application.Services
{
    public class LEDService : ILEDService, IDisposable
    {
        private readonly GpioController gpioController;
        private int[] leds = new int[] {
                5, 19, 6
            };

        public LEDService(GpioController gpioController)
        {
            this.gpioController = gpioController;

            foreach (var led in leds)
            {
                gpioController.OpenPin(led);
                gpioController.SetPinMode(led, PinMode.Output);
                gpioController.Write(led, PinValue.Low);
            }
        }

        public void Dispose()
        {
            foreach (var led in leds)
            {
                gpioController.ClosePin(led);
            }
        }

        public Task SetAsync(int id, bool state)
        {
            return Task.Run(() => gpioController.Write(leds[id], state));
        }

        public Task<bool> ToggleAsync(int id) => Task.FromResult(gpioController.Read(leds[id]) == PinValue.High);
    }
}
