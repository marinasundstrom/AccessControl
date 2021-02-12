using System;
using System.Threading.Tasks;

namespace AccessPoint.Services
{
    public interface IRfidReader : IDisposable
    {
        IObservable<CardData> WhenCardDetected { get; }
        Task<CardData> ReadCardUniqueIdAsync();
        Task StartAsync();
        Task StopAsync();
    }
}
