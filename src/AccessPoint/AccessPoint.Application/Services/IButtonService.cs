using System;

namespace AccessPoint.Application.Services
{
    public interface IButtonService
    {
        long InterruptTime { get; }

        event EventHandler Pressed;
        event EventHandler Released;

        void Dispose();
    }
}