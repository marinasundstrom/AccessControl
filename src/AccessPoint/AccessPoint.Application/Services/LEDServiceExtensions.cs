using System;
using System.Threading;
using System.Threading.Tasks;

namespace AccessPoint.Application.Services
{
    public static class LEDServiceExtensions
    {
        private const int RedLED = 0;
        private const int GreenLED = 1;
        private const int BlueLED = 2;

        public static async Task ToggleAllLedsOff(this ILEDService ledService)
        {
            await ledService.SetAsync(GreenLED, false);
            await ledService.SetAsync(RedLED, false);
            await ledService.SetAsync(BlueLED, false);
        }

        public static async Task ToggleRedLedOn(this ILEDService ledService)
        {
            await ledService.SetAsync(RedLED, true);
            await ledService.SetAsync(GreenLED, false);
            await ledService.SetAsync(BlueLED, false);
        }

        public static async Task ToggleGreenLedOn(this ILEDService ledService)
        {
            await ledService.SetAsync(RedLED, false);
            await ledService.SetAsync(GreenLED, true);
            await ledService.SetAsync(BlueLED, false);
        }

        public static async Task ToggleBlueLedOn(this ILEDService ledService)
        {
            await ledService.SetAsync(RedLED, false);
            await ledService.SetAsync(GreenLED, false);
            await ledService.SetAsync(BlueLED, true);
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
