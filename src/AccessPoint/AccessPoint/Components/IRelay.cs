using System;

namespace AccessPoint.Components
{
    public interface IRelay : IDisposable
    {
        int InPin { get; }
        bool GetRelayState();
        void SetRelayState(bool value);
    }
}