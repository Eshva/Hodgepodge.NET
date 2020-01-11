#region Usings

using Eshva.Common.WebApp.ApplicationConfiguration;
using Eshva.Polls.Configuration.WebService.Client;
using Microsoft.Extensions.Configuration;
using IConfiguration = Eshva.Common.WebApp.ApplicationConfiguration.IConfiguration;

#endregion


namespace Eshva.Polls.Admin.WebApp.Configuration
{
    public sealed class WebServicesConfiguration : IWebServicesConfiguration, IConfiguration
    {
        public ProductConfigurationWebServiceConfiguration ProductConfigs { get; private set; }

        void IConfiguration.LoadFrom(IConfigurationSection configurationSection)
        {
            ProductConfigs = configurationSection.GetSection("ProductConfigs")
                                                 .LoadConfiguration<ProductConfigurationWebServiceConfiguration>();
        }

        void IConfiguration.Validate()
        {
            ((IConfiguration)ProductConfigs).Validate();
        }
    }
}
