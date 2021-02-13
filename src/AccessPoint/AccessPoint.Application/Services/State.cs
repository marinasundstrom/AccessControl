using System;
using System.Threading;

namespace AccessPoint.Application.Services
{
    public class State
    {
        public bool Authenticated;
        public bool Unlocked;

        public int LockRelay = 0;

        public bool LockWhenShut = true;
        public bool ArmWhenShut = true;

        public TimeSpan AccessTime = TimeSpan.FromSeconds(10); // 30 seconds
        public TimeSpan BuzzTime = TimeSpan.FromSeconds(5);

        public Timer Timer;
    }
}
