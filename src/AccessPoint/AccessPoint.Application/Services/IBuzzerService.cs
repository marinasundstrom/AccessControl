using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccessPoint.Application.Services
{
    public interface IBuzzerService
    {
        Task BuzzAsync(TimeSpan duration);

        Task BuzzAsync(TimeSpan duration, CancellationToken cancellationToken = default);

        void Stop();
    }
}
