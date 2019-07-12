using System;

namespace Foobiq.AccessPoint.Services
{
    public interface ISwitchService : IDisposable
    {
        event EventHandler Closed;
        event EventHandler Opened;
    }
}