using System;
using System.Collections.Generic;
using System.Text;

namespace AccessControl.Messages.Commands
{
    public class DisarmCommand : Command
    {
        public const string DisarmCommandConstant = "Disarm";

        public DisarmCommand() : base(DisarmCommandConstant)
        {
        }
    }
}
