namespace AccessControl.Messages.Commands
{
    public class ArmCommand : Command
    {
        public const string ArmCommandConstant = "Arm";

        public ArmCommand() : base(ArmCommandConstant)
        {
        }
    }
}
