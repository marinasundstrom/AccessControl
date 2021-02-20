using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace AccessPoint.Application.Services
{
    public partial interface ILEDService : IDisposable
    {
        Task SetColorAsync(byte r, byte g, byte b);
        Task SetColorAsync(Color color);
    }
}
