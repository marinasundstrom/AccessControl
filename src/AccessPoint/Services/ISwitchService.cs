using System;

namespace AccessPoint.Services
{
    public interface ISwitchService : IDisposable
    {
        event EventHandler Closed;
        event EventHandler Opened;
    }
}