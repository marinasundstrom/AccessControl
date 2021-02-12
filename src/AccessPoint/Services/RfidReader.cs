using System;
using System.Collections.Generic;
using System.Device.Spi;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using AccessPoint.Components.Mfrc522;
using Microsoft.Extensions.Logging;

namespace AccessPoint.Services
{
    public class RfidReader : IRfidReader
    {
        private readonly Mfrc522Controller _rfidController;
        private readonly Subject<CardData> _whenCardDetected;
        private readonly ILogger<RfidReader> _logger;
        private Timer _timer;

        public RfidReader(ILogger<RfidReader> logger, SpiDevice spiDevice)
        {
            _rfidController = new Mfrc522Controller(spiDevice);

            _whenCardDetected = new Subject<CardData>();
            _logger = logger;
        }

        public Task StartAsync()
        {
            _logger.LogInformation("RFID Service is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromMilliseconds(1000));

            _logger.LogInformation("RFID Reader Service has started.");

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var (status, _) = _rfidController.Request(RequestMode.RequestIdle);

            if (status != Status.OK)
                return;

            var (status2, uid) = _rfidController.AntiCollision();

            if (status2 != Status.OK)
                return;

            _logger.LogInformation($"UID read: {string.Join(", ", uid)}");

            _whenCardDetected.OnNext(new CardData(uid));
        }

        public Task StopAsync()
        {
            _logger.LogInformation("RFID Reader Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public Task<CardData> ReadCardUniqueIdAsync()
        {
            var (status, uid) = _rfidController.AntiCollision();

            return Task.FromResult(new CardData(uid));
        }

        public IObservable<CardData> WhenCardDetected => _whenCardDetected;
    }
}
