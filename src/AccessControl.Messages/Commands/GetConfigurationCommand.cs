namespace AccessControl.Messages.Commands
{
    public class GetConfigurationCommand : Command
    {
        public const string GetConfigurationCommandConstant = "GetConfiguration";

        public GetConfigurationCommand() : base(GetConfigurationCommandConstant)
        {
        }
    }
}
