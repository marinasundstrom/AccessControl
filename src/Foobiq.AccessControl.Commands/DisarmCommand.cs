using System;
using System.Collections.Generic;
using System.Text;

namespace Foobiq.AccessControl.Commands
{
    public class DisarmCommand : Command
    {
        public const string DisarmCommandConstant = "Disarm";

        public DisarmCommand() : base(DisarmCommandConstant)
        {
        }
    }
}
