namespace AccessControl
{
    public interface IResourceContainer
    {
        string GetString(string key);
        string GetString(string key, params object[] args);
    }
}
