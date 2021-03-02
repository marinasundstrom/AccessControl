using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AccessControl.Messages.Events
{
    public class AlarmEvent : Event
    {
        public const string EventNameConstant = "AlarmEvent";

        public AlarmEvent(AlarmState alarmState, bool rex = false) : base(EventNameConstant)
        {
            AlarmState = alarmState;
            Rex = rex;
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public AlarmState AlarmState { get; }

        public bool Rex { get; }
    }
}
