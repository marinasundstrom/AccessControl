using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Foobiq.AccessControl.Commands
{
    public class Command
    {
        public Command(string commandName, IDictionary<string, object> args = null)
        {
            CommandName = commandName;
            Args = args;
        }

        [JsonProperty("Command")]
        public string CommandName { get; private set; }

        public IDictionary<string, object> Args { get; }
    }
}
