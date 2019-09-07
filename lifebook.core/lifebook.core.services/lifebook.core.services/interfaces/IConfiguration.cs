using System;
namespace lifebook.core.services.interfaces
{
    public interface IConfiguration
    {
        string this[string key] { get; }
        string GetValue(string key);
        T GetValue<T>(string key);
        T TryGetValueOrDefault<T>(string key, T defaultValue);
    }
}
