namespace AccessControl.Contracts.Events
{
    public class AlarmEvent
    {
        public AlarmEvent()
        {

        }

        public AlarmEvent(AlarmState alarmState, bool rex = false)
        {
            AlarmState = alarmState;
            Rex = rex;
        }

        public AlarmState AlarmState { get; }

        public bool Rex { get; }
    }
}
