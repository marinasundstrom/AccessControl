using System;
using System.Device.Pwm.Drivers;
using System.Drawing;

namespace AccessPoint.Application.Components
{
    public class RgbLed : IRgbLed
    {
        private SoftwarePwmChannel redChannel;
        private SoftwarePwmChannel greenChannel;
        private SoftwarePwmChannel blueChannel;

        public RgbLed(int redPin = 4, int greenPin = 19, int bluePin = 6)
        {
            redChannel = new SoftwarePwmChannel(redPin, 400, 1);
            greenChannel = new SoftwarePwmChannel(greenPin, 400, 1);
            blueChannel = new SoftwarePwmChannel(bluePin, 400, 1);

            redChannel.Start();
            greenChannel.Start();
            blueChannel.Start();
        }

        public void Dispose()
        {
            redChannel.Dispose();
            greenChannel.Dispose();
            blueChannel.Dispose();
        }

        public void SetColor(Color color)
        {
            SetColor(color.R, color.G, color.B);
        }

        public void SetColor(byte r, byte g, byte b)
        {
            redChannel.DutyCycle = r / 255D;
            greenChannel.DutyCycle = g / 255D;
            blueChannel.DutyCycle = b / 255D;
        }

        public void TurnOff()
        {
            SetColor(0, 0, 0);
        }
    }
}
