using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Threading.Tasks;

namespace AccessPoint.Application.Services
{
    public sealed class PirSensorService : IPirSensorService
    {
        private readonly GpioController gpioController;
        private const int InPinNumber = 17;
        private DateTime _pressedLastInterrupt;
        private DateTime _releasedLastInterrupt;

        public PirSensorService(GpioController gpioController)
        {
            this.gpioController = gpioController;

            gpioController.OpenPin(InPinNumber);
            gpioController.SetPinMode(InPinNumber, PinMode.InputPullDown);
            gpioController.RegisterCallbackForPinValueChangedEvent(InPinNumber, PinEventTypes.Falling, OnPinChanged);
            gpioController.RegisterCallbackForPinValueChangedEvent(InPinNumber, PinEventTypes.Rising, OnPinChanged);
        }

        public long InterruptTime { get; } = 100;

        /// <summary>
        /// Occurs when [MotionNotDetected].
        /// </summary>
        public event EventHandler MotionNotDetected;

        /// <summary>
        /// Occurs when [MotionDetected].
        /// </summary>
        public event EventHandler MotionDetected;

        private void OnPinChanged(object sender, PinValueChangedEventArgs pinValueChangedEventArgs)
        {
            if (pinValueChangedEventArgs.ChangeType == PinEventTypes.Rising)
            {
                HandleCircuitOpen();
            }
            else if (pinValueChangedEventArgs.ChangeType == PinEventTypes.Falling)
            {
                HandleCircuitClosed();
            }
        }

        private void HandleCircuitClosed()
        {
            DateTime interruptTime = DateTime.Now;

            if ((interruptTime - _pressedLastInterrupt).TotalMilliseconds <= InterruptTime) return;
            _pressedLastInterrupt = interruptTime;
            MotionNotDetected?.Invoke(this, EventArgs.Empty);
        }

        private void HandleCircuitOpen()
        {
            DateTime interruptTime = DateTime.Now;

            if ((interruptTime - _releasedLastInterrupt).TotalMilliseconds <= InterruptTime) return;
            _releasedLastInterrupt = interruptTime;
            MotionDetected?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            gpioController.UnregisterCallbackForPinValueChangedEvent(InPinNumber, OnPinChanged);
            gpioController.ClosePin(InPinNumber);
        }
    }
}
