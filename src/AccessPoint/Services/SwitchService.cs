using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Threading.Tasks;

namespace AccessPoint.Services
{
    public sealed class SwitchService : ISwitchService
    {
        private readonly GpioController gpioController;
        private const int InPinNumber = 26;
        private DateTime _pressedLastInterrupt;
        private DateTime _releasedLastInterrupt;

        public SwitchService(GpioController gpioController)
        {
            this.gpioController = gpioController;

            gpioController.OpenPin(InPinNumber);
            gpioController.SetPinMode(InPinNumber, PinMode.InputPullDown);
            gpioController.RegisterCallbackForPinValueChangedEvent(InPinNumber, PinEventTypes.Falling, OnPinChanged);
            gpioController.RegisterCallbackForPinValueChangedEvent(InPinNumber, PinEventTypes.Rising, OnPinChanged);
        }

        public long InterruptTime { get; } = 100;

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
            if(pinValueChangedEventArgs.ChangeType == PinEventTypes.Rising)
            {
                HandleCircuitClosed();
            }
            else if (pinValueChangedEventArgs.ChangeType == PinEventTypes.Falling)
            {
                HandleCircuitOpen();
            }
        }

        private void HandleCircuitClosed()
        {
            DateTime interruptTime = DateTime.Now;

            if ((interruptTime - _pressedLastInterrupt).TotalMilliseconds <= InterruptTime) return;
            _pressedLastInterrupt = interruptTime;
            Closed?.Invoke(this, EventArgs.Empty);
        }

        private void HandleCircuitOpen()
        {
            DateTime interruptTime = DateTime.Now;

            if ((interruptTime - _releasedLastInterrupt).TotalMilliseconds <= InterruptTime) return;
            _releasedLastInterrupt = interruptTime;
            Opened?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
           gpioController.UnregisterCallbackForPinValueChangedEvent(InPinNumber, OnPinChanged);
           gpioController.ClosePin(InPinNumber);
        }
    }
}
