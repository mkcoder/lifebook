using System;
namespace lifebook.core.services.interfaces
{
    public interface IConfiguration
    {
        string GetValue(string key);
        T TryGetValueOrDefault<T>(string key, T defaultValue);
    }
}
