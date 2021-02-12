using System;
using System.Device.Gpio;
using System.Device.Pwm;
using System.Threading;
using System.Threading.Tasks;

namespace AccessPoint.Components
{
    public sealed class Buzzer : IBuzzer
    {
        private readonly GpioController gpioController;
        private readonly PwmController pwmController;
        private CancellationTokenSource cts;

        public Buzzer(
            GpioController gpioController,
            PwmController pwmController,
            int outPin,
            int toneFrequency = 523)
        {
            this.gpioController = gpioController;
            this.pwmController = pwmController;

            OutPin = outPin;
            ToneFrequency = toneFrequency;
        }

        public int ToneFrequency { get; set; } = 2000;

        public int OutPin { get; }

        public async Task BuzzAsync(CancellationToken cancellationToken = default) =>
            await BuzzAsync(Timeout.InfiniteTimeSpan, cancellationToken);

        public async Task BuzzAsync(TimeSpan time, CancellationToken cancellationToken = default)
        {
            cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            if (!gpioController.IsPinOpen(OutPin))
            {
                gpioController.OpenPin(OutPin);
                pwmController.OpenChannel(OutPin, 0);
            }

            pwmController.StartWriting(OutPin, 0, ToneFrequency, 1);

            try
            {
                pwmController.ChangeDutyCycle(OutPin, 0, 50);
                await Task.Delay(time, cts.Token);
            }
            catch (TaskCanceledException)
            {
                // Just ignore. It's OK.
            }
            finally
            {
                pwmController.ChangeDutyCycle(OutPin, 0, 0);
                pwmController.StopWriting(OutPin, 0);

                cts = null;
            }
        }

        public void Dispose()
        {
            pwmController.CloseChannel(OutPin, 0);
            gpioController.ClosePin(OutPin);
        }

        public void Stop()
        {

            cts?.Cancel();
        }
    }
}
