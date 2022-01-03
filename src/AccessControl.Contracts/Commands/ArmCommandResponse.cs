namespace AccessControl.Contracts.Commands
{
    public class ArmCommandResponse
    {
        public ArmCommandResponse()
        {
        }

        public ArmCommandResponse(AlarmState alarmState)
        {
            AlarmState = alarmState;
        }

        public AlarmState AlarmState { get; set; }
    }
}
