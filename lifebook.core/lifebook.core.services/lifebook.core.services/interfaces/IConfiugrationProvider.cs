namespace lifebook.core.services.interfaces
{
    public interface IConfigurationProvider
    {
        string this[string key] { get; }
    }
}
