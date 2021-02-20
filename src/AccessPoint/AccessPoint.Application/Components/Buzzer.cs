using System;
using System.Device.Gpio;
using System.Device.Pwm;
using System.Threading;
using System.Threading.Tasks;

namespace AccessPoint.Application.Components
{
    public sealed class Buzzer : IBuzzer
    {
        private CancellationTokenSource cts;
        private PwmChannel pwmChannel;

        public Buzzer(
            int channel,
            int toneFrequency = 523)
        {
            Channel = channel;
            ToneFrequency = toneFrequency;
        }

        public int Channel { get; }
        public int ToneFrequency { get; set; } = 2000;

        public async Task BuzzAsync(CancellationToken cancellationToken = default) =>
            await BuzzAsync(Timeout.InfiniteTimeSpan, cancellationToken);

        public async Task BuzzAsync(TimeSpan time, CancellationToken cancellationToken = default)
        {
            cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            pwmChannel = PwmChannel.Create(0, Channel, ToneFrequency, 0.5);
            pwmChannel.Start();

            try
            {
                await Task.Delay(time, cts.Token);
            }
            catch (TaskCanceledException)
            {
                // Just ignore. It's OK.
            }
            finally
            {
                pwmChannel.Stop();

                cts = null;
            }
        }

        public void Dispose()
        {
            pwmChannel.Stop();
        }

        public void Stop()
        {
            pwmChannel.Dispose();
            cts?.Cancel();
        }
    }
}
