using System;

namespace Foobiq.AccessPoint.Components
{
    public interface IRelay : IDisposable
    {
        int InPin { get; }
        bool GetRelayState();
        void SetRelayState(bool value);
    }
}