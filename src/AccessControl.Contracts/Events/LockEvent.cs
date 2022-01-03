namespace AccessControl.Contracts.Events
{
    public class LockEvent
    {
        public LockEvent()
        {
            
        }

        public LockEvent(LockState lockState)
        {
            LockState = lockState;
        }

        public LockState LockState { get; }
    }
}
