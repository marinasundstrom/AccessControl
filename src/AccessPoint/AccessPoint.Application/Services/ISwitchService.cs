using System;

namespace AccessPoint.Application.Services
{
    public interface ISwitchService : IDisposable
    {
        event EventHandler Closed;
        event EventHandler Opened;
    }
}