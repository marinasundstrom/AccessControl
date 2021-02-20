using System;
using System.Device.Gpio;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using AccessPoint.Application.Components;

namespace AccessPoint.Application.Services
{
    public class LEDService : ILEDService
    {
        private RgbLed rgbLed;

        public LEDService()
        {
            this.rgbLed = new RgbLed();
        }

        public void Dispose()
        {
            rgbLed.Dispose();
        }

        public Task SetColorAsync(Color color)
        {
            rgbLed.SetColor(color);

            return Task.CompletedTask;
        }

        public Task SetColorAsync(byte r, byte g, byte b)
        {
            rgbLed.SetColor(r, g, b);

            return Task.CompletedTask;
        }
    }
}
