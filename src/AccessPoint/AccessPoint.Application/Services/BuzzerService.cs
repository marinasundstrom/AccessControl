using System;
using System.Device.Gpio;
using System.Device.Pwm;
using System.Threading.Tasks;
using AccessPoint.Application.Components;

namespace AccessPoint.Application.Services
{
    public class BuzzerService : IBuzzerService
    {
        private readonly Buzzer buzzer;

        public BuzzerService(GpioController gpioController)
        {
            buzzer = new Buzzer(0);
        }

        public Task BuzzAsync(TimeSpan duration) => buzzer.BuzzAsync(duration);

        public void Stop() => buzzer.Stop();
    }
}
