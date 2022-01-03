namespace AccessControl.Contracts.Commands
{
    public class GetAlarmStateCommandResponse
    {
        public GetAlarmStateCommandResponse()
        {
        }

        public GetAlarmStateCommandResponse(AlarmState alarmState)
        {
            AlarmState = alarmState;
        }

        public AlarmState AlarmState { get; set; }
    }
}
