using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Foobiq.AccessControl.Events
{
    public class LockEvent : Event
    {
        public const string EventNameConstant = "LockEvent";

        public LockEvent(LockState lockState) : base(EventNameConstant)
        {
            LockState = lockState;
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public LockState LockState { get; }
    }
}
