using System;
using System.Threading.Tasks;

namespace AccessPoint.Application.Services
{
    public interface IRfidReader : IDisposable
    {
        IObservable<CardData> WhenCardDetected { get; }
        Task<CardData> ReadCardUniqueIdAsync();
        Task StartAsync();
        Task StopAsync();
    }
}
