using System;
using System.Threading.Tasks;

namespace AccessPoint.Application.Components
{
    public interface ISwitch
    {
        int InPin { get; }
        ulong InterruptTime { get; }

        event EventHandler<EventArgs> Closed;
        event EventHandler<EventArgs> Open;

        Task<bool> GetStateAsync();
    }
}
