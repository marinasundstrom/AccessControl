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

        public ButtonService(GpioController gpioController)
        {
            this.gpioController = gpioController;

            gpioController.OpenPin(InPinNumber);
            gpioController.SetPinMode(InPinNumber, PinMode.InputPullDown);
            gpioController.RegisterCallbackForPinValueChangedEvent(InPinNumber, PinEventTypes.Falling, OnPinChanged);
            gpioController.RegisterCallbackForPinValueChangedEvent(InPinNumber, PinEventTypes.Rising, OnPinChanged);
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
            if (pinValueChangedEventArgs.ChangeType == PinEventTypes.Rising)
            {
                HandlePushed();
            }
            else if (pinValueChangedEventArgs.ChangeType == PinEventTypes.Falling)
            {
                HandleReleased();
            }
        }

        private void HandlePushed()
        {
            DateTime interruptTime = DateTime.Now;

            if ((interruptTime - _pressedLastInterrupt).TotalMilliseconds <= InterruptTime) return;
            _pressedLastInterrupt = interruptTime;
            Pressed?.Invoke(this, EventArgs.Empty);
        }

        private void HandleReleased()
        {
            DateTime interruptTime = DateTime.Now;

            if ((interruptTime - _releasedLastInterrupt).TotalMilliseconds <= InterruptTime) return;
            _releasedLastInterrupt = interruptTime;
            Released?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            gpioController.UnregisterCallbackForPinValueChangedEvent(InPinNumber, OnPinChanged);
            gpioController.ClosePin(InPinNumber);
        }
    }
}
