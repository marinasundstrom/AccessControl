using System;

namespace AccessPoint.Application.Services
{
    public interface IPirSensorService
    {
        long InterruptTime { get; }

        event EventHandler MotionNotDetected;
        event EventHandler MotionDetected;

        void Dispose();
    }
}