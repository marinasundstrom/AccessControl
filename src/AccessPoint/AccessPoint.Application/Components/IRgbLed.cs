using System;
using System.Drawing;

namespace AccessPoint.Application.Components
{
    public interface IRgbLed : IDisposable
    {
        void SetColor(Color color);
        void SetColor(byte r, byte g, byte b);
        void TurnOff();
    }
}