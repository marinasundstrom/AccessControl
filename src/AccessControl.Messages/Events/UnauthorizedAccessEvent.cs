namespace AccessControl.Messages.Events
{

    public class UnauthorizedAccessEvent : Event
    {
        public const string EventNameConstant = "UnauthorizedAccess";

        public UnauthorizedAccessEvent() : base(EventNameConstant)
        {

        }
    }
}
