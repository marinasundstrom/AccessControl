using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace AccessPoint.Application.Services
{
    public static class LEDServiceExtensions
    {
        public static async Task ToggleOff(this ILEDService ledService)
        {
            await ledService.SetColorAsync(0, 0, 0);
        }

        public static async Task ToggleRedLedOn(this ILEDService ledService)
        {
            await ledService.SetColorAsync(Color.Red);
        }

        public static async Task ToggleGreenLedOn(this ILEDService ledService)
        {
            await ledService.SetColorAsync(Color.Green);
        }

        public static async Task ToggleBlueLedOn(this ILEDService ledService)
        {
            await ledService.SetColorAsync(Color.Blue);
        }

        public static async Task ToggleTimedColor(this ILEDService ledService, Color color, TimeSpan timeSpan, CancellationToken cancellationToken = default)
        {
            try
            {
                await ledService.SetColorAsync(color);

                await Task.Delay(timeSpan, cancellationToken);
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                await ledService.ToggleOff();
            }
        }

        public static Task Blink(this ILEDService ledService, int times, Color color, Action callback = null, CancellationToken cancellationToken = default)
        {
            return Blink(ledService, times, color, TimeSpan.FromSeconds(1), callback, cancellationToken);
        }

        public static Task Blink(this ILEDService ledService, int times, Color color, TimeSpan delay, Action callback = null, CancellationToken cancellationToken = default)
        {
            return Task.Run(async () =>
            {
                for (int i = 0; i < times; i++)
                {
                    if (cancellationToken.IsCancellationRequested)
                        return;

                    await ledService.SetColorAsync(color);

                    callback?.Invoke();

                    await Task.Delay(delay);

                    await ledService.ToggleOff();

                    await Task.Delay(delay);
                }
            });
        }
    }
}
