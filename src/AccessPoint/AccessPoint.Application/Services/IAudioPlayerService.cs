using System;
using System.Threading.Tasks;

namespace AccessPoint.Application.Services
{
    public interface IAudioPlayerService : IDisposable
    {
        Task Play(string fileName);
    }
}