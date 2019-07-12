using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Foobiq.AccessControl.Events
{
    public class Event
    {
        public Event(string eventName)
        {
            EventName = eventName;
        }

        [JsonProperty("Event")]
        public string EventName { get; private set; }
    }
}
