namespace AccessControl.Messages.Commands
{
    public class ReadTagCommand : Command
    {
        public const string ReadTagCommandConstant = "ReadTag";

        public ReadTagCommand() : base(ReadTagCommandConstant)
        {
        }
    }
}
