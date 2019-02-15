using Microsoft.Extensions.Configuration;

namespace lifebook.core.services.configuration
{
    public interface IConfigurationProviderInistalizer
    {
        void Provide(IConfigurationBuilder configurationBuilder);
    }
}