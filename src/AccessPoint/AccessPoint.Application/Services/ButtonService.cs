using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Threading.Tasks;

namespace AccessPoint.Application.Services
{
    public sealed class ButtonService : IButtonService
    {
        private readonly GpioController gpioController;
        private const int InPinNumber = 23;
        private DateTime _pressedLastInterrupt;
        private DateTime _releasedLastInterrupt;
        private DateTime triggerTimestamp;

        public ButtonService(GpioController gpioController)
        {
            this.gpioController = gpioController;

            gpioController.OpenPin(InPinNumber);
            gpioController.SetPinMode(InPinNumber, PinMode.InputPullDown);
            gpioController.RegisterCallbackForPinValueChangedEvent(InPinNumber, PinEventTypes.Falling | PinEventTypes.Rising, OnPinChanged);
        }

        public long InterruptTime { get; } = 500;

        /// <summary>
        /// Occurs when [pushed].
        /// </summary>
        public event EventHandler Pressed;

        /// <summary>
        /// Occurs when [released].
        /// </summary>
        public event EventHandler Released;

        private void OnPinChanged(object sender, PinValueChangedEventArgs pinValueChangedEventArgs)
        {
            var value = gpioController.Read(InPinNumber);

            var duration = DateTime.Now - triggerTimestamp;
            if (duration.TotalMilliseconds < InterruptTime) return;

            if (value == PinValue.Low)
            {
                triggerTimestamp = DateTime.Now;

                Released?.Invoke(this, EventArgs.Empty);
            }
            else if (value == PinValue.High)
            {
                triggerTimestamp = DateTime.Now;

                Pressed?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Dispose()
        {
            gpioController.UnregisterCallbackForPinValueChangedEvent(InPinNumber, OnPinChanged);
            gpioController.ClosePin(InPinNumber);
        }
    }
}
