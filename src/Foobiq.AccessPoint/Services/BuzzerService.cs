using Foobiq.AccessPoint.Components;
using System;
using System.Device.Gpio;
using System.Device.Pwm;
using System.Threading.Tasks;

namespace Foobiq.AccessPoint.Services
{
    public class BuzzerService : IBuzzerService
    {
        private readonly Buzzer buzzer;

        public BuzzerService(GpioController gpioController, PwmController pwmController)
        {
            buzzer = new Buzzer(gpioController, pwmController, 13);
        }

        public Task BuzzAsync(TimeSpan duration) => buzzer.BuzzAsync(duration);

        public void Stop() => buzzer.Stop();
    }
}
