using System;
using System.IO;
using System.Threading.Tasks;
using Iot.Device.Media;

namespace AccessPoint.Application.Services
{
    public class AudioPlayerService : IAudioPlayerService
    {
        private SoundDevice device;

        public AudioPlayerService()
        {
            SoundConnectionSettings settings = new SoundConnectionSettings();
            device = SoundDevice.Create(settings);
            device.PlaybackVolume = 50L;
        }

        public Task Play(string fileName)
        {
            return Task.Run(() =>
            {
                device.Play(Path.Combine(Environment.CurrentDirectory, "Assets/" + fileName));
            });
        }

        public void Dispose()
        {
            device.Dispose();
        }
    }
}
