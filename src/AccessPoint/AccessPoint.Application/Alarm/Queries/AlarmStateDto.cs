using AccessControl.Messages.Commands;

namespace AccessPoint.Application.Alarm.Queries
{
    public class AlarmStateDto
    {
        public AlarmStateDto(AlarmState alarmState)
        {
            AlarmState = alarmState;
        }

        public AlarmState AlarmState { get; set; }
    }
}