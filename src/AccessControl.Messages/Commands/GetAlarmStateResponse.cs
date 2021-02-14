using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AccessControl.Messages.Commands
{
    public class GetAlarmStateResponse
    {
        public GetAlarmStateResponse()
        {
        }

        public GetAlarmStateResponse(AlarmState alarmState)
        {
            AlarmState = alarmState;
        }

        public AlarmState AlarmState { get; set; }
    }
}
