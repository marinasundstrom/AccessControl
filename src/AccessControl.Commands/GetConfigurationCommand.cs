namespace AccessControl.Commands
{
    public class GetConfigurationCommand : Command
    {
        public const string GetConfigurationCommandConstant = "GetConfiguration";

        public GetConfigurationCommand() : base(GetConfigurationCommandConstant)
        {
        }
    }
}
