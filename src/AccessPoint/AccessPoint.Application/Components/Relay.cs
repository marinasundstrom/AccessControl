using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Threading.Tasks;

namespace AccessPoint.Application.Components
{
    public class Relay : IRelay
    {
        private readonly GpioController gpioController;

        public Relay(GpioController gpioController, int inPin)
        {
            this.gpioController = gpioController;
            InPin = inPin;

            if (!gpioController.IsPinOpen(InPin))
            {
                gpioController.OpenPin(InPin);
            }
            gpioController.SetPinMode(InPin, PinMode.Output);
            gpioController.Write(InPin, PinValue.High);
        }

        public int InPin { get; }

        public void SetRelayState(bool value) => gpioController.Write(InPin, value ? PinValue.High : PinValue.Low);

        public bool GetRelayState() => gpioController.Read(InPin) != PinValue.High;

        public void Dispose()
        {
            gpioController.ClosePin(InPin);
        }
    }
}
