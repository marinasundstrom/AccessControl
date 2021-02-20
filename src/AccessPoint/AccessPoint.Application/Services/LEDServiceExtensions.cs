using System;
using System.Threading;
using System.Threading.Tasks;

namespace AccessPoint.Application.Services
{
    public static class LEDServiceExtensions
    {
        public static async Task ToggleAllLedsOff(this ILEDService ledService)
        {
            await ledService.SetColorAsync(0, 0, 0);
        }

        public static async Task ToggleRedLedOn(this ILEDService ledService)
        {
            await ledService.SetColorAsync(255, 0, 0);

        }

        public static async Task ToggleGreenLedOn(this ILEDService ledService)
        {
            await ledService.SetColorAsync(0, 255, 0);

        }

        public static async Task ToggleBlueLedOn(this ILEDService ledService)
        {
            await ledService.SetColorAsync(0, 0, 255);

        }

        public static async Task BlinkBlue(this ILEDService ledService, IBuzzerService buzzerService, CancellationToken cancellationToken)
        {
            await Task.Delay(1500);

            for (int i = 0; i < 5; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                await ledService.ToggleBlueLedOn();

                await buzzerService.BuzzAsync(TimeSpan.FromSeconds(1));

                await ledService.ToggleAllLedsOff();

                await Task.Delay(TimeSpan.FromSeconds(1));

                if (cancellationToken.IsCancellationRequested)
                    return;
            }
        }

    }
}
