using AccessControl.Contracts.Commands;

namespace AccessPoint.Application.Lock.Queries
{
    public class LockStateDto
    {
        public LockStateDto(LockState lockState)
        {
            LockState = lockState;
        }

        public LockState LockState { get; set; }
    }
}
