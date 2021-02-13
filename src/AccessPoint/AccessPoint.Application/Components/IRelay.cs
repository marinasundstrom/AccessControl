using System;

namespace AccessPoint.Application.Components
{
    public interface IRelay : IDisposable
    {
        int InPin { get; }
        bool GetRelayState();
        void SetRelayState(bool value);
    }
}