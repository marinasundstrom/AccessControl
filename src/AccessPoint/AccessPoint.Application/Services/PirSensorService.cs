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
        private DateTime triggerTimestamp;

        public PirSensorService(GpioController gpioController)
        {
            this.gpioController = gpioController;

            gpioController.OpenPin(InPinNumber);
            gpioController.SetPinMode(InPinNumber, PinMode.InputPullDown);
            gpioController.RegisterCallbackForPinValueChangedEvent(InPinNumber, PinEventTypes.Falling | PinEventTypes.Rising, OnPinChanged);
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
            var value = gpioController.Read(InPinNumber);

            var duration = DateTime.Now - triggerTimestamp;
            if (duration.TotalMilliseconds < InterruptTime) return;

            //Console.WriteLine(duration);

            if (value == PinValue.Low)
            {
                triggerTimestamp = DateTime.Now;

                MotionNotDetected?.Invoke(this, EventArgs.Empty);

            }
            else if (value == PinValue.High)
            {
                triggerTimestamp = DateTime.Now;

                MotionDetected?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Dispose()
        {
            gpioController.UnregisterCallbackForPinValueChangedEvent(InPinNumber, OnPinChanged);
            gpioController.ClosePin(InPinNumber);
        }
    }
}
