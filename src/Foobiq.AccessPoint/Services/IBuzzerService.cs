using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foobiq.AccessPoint.Services
{
    public interface IBuzzerService
    {
        Task BuzzAsync(TimeSpan duration);

        void Stop();
    }
}
