using System;
using System.Threading;
using System.Threading.Tasks;

namespace AccessPoint.Application.Components
{
    public interface IBuzzer : IDisposable
    {
        int Channel { get; }
        int ToneFrequency { get; set; }

        Task BuzzAsync(CancellationToken cancellationToken = default);
        Task BuzzAsync(TimeSpan time, CancellationToken cancellationToken = default);
        void Stop();
    }
}