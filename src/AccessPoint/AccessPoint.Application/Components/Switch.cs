using System;
using System.Device.Gpio;
using System.Threading.Tasks;

namespace AccessPoint.Application.Components
{
    public sealed class Switch : ISwitch
    {
        private GpioController gpioController;
        private DateTime _pressedLastInterrupt;
        private DateTime _releasedLastInterrupt;

        /// <summary>
        /// Initializes a new instance of the <see cref="Switch"/> class.
        /// </summary>
        /// <param name="pin">The gpio pin.</param>
        public Switch(
            GpioController gpioController,
            int inPin,
            ulong interruptTime = 100)
        {
            this.gpioController = gpioController;
            this.InPin = inPin;

            gpioController.OpenPin(InPin);
            gpioController.SetPinMode(InPin, PinMode.InputPullDown);
            gpioController.RegisterCallbackForPinValueChangedEvent(InPin, PinEventTypes.Falling, HandleInterrupt);
            gpioController.RegisterCallbackForPinValueChangedEvent(InPin, PinEventTypes.Rising, HandleInterrupt);

            InterruptTime = interruptTime;
        }

        public int InPin { get; }   

        public ulong InterruptTime { get; }

        /// <summary>
        /// Occurs when [closed].
        /// </summary>
        public event EventHandler<EventArgs> Closed;

        /// <summary>
        /// Occurs when [open].
        /// </summary>
        public event EventHandler<EventArgs> Open;

        private void HandleInterrupt(object sender, PinValueChangedEventArgs pinValueChangedEventArgs)
        {
            if (pinValueChangedEventArgs.ChangeType == PinEventTypes.Rising)
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
            Open?.Invoke(this, EventArgs.Empty);
        }

        public Task<bool> GetStateAsync() => Task.Run(() => gpioController.Read(InPin) == PinValue.High);
    }
}
