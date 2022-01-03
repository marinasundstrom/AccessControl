namespace AccessControl.Contracts.Commands
{
    public class DisarmCommandResponse
    {
        public DisarmCommandResponse()
        {
        }

        public DisarmCommandResponse(AlarmState alarmState)
        {
            AlarmState = alarmState;
        }

        public AlarmState AlarmState { get; set; }
    }
}
