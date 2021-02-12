namespace AccessControl.Messages.Events
{
    public class AccessEvent : Event
    {
        public const string EventNameConstant = "Access";

        public AccessEvent() : base(EventNameConstant)
        {

        }
    }
}
