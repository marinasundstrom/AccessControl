using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Threading.Tasks;

namespace AccessPoint.Application.Services
{
    public sealed class SwitchService : ISwitchService
    {
        private readonly GpioController gpioController;
        private const int InPinNumber = 26;
        private DateTime triggerTimestamp;

        public SwitchService(GpioController gpioController)
        {
            this.gpioController = gpioController;

            gpioController.OpenPin(InPinNumber);
            gpioController.SetPinMode(InPinNumber, PinMode.InputPullDown);
            gpioController.RegisterCallbackForPinValueChangedEvent(InPinNumber, PinEventTypes.Falling | PinEventTypes.Rising, OnPinChanged);
        }

        public long InterruptTime { get; } = 700;

        /// <summary>
        /// Occurs when [closed].
        /// </summary>
        public event EventHandler Closed;

        /// <summary>
        /// Occurs when [open].
        /// </summary>
        public event EventHandler Opened;

        private void OnPinChanged(object sender, PinValueChangedEventArgs pinValueChangedEventArgs)
        {
            var value = gpioController.Read(InPinNumber);

            var duration = DateTime.Now - triggerTimestamp;
            if (duration.TotalMilliseconds < InterruptTime) return;

            if (value == PinValue.Low)
            {
                triggerTimestamp = DateTime.Now;

                Opened?.Invoke(this, EventArgs.Empty);

            }
            else if (value == PinValue.High)
            {
                triggerTimestamp = DateTime.Now;

                Closed?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Dispose()
        {
            gpioController.UnregisterCallbackForPinValueChangedEvent(InPinNumber, OnPinChanged);
            gpioController.ClosePin(InPinNumber);
        }
    }
}
