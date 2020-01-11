#region Usings

using Eshva.Common.WebApp.ApplicationConfiguration;
using Eshva.Polls.Configuration.Contracts.Client;
using Eshva.Polls.Configuration.WebService.Client;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

#endregion


namespace Eshva.Polls.Admin.WebApp.Configuration
{
    public static class ApplicationConfigurationBootstrapping
    {
        public static void AddApplicationConfiguration(this Container container, IConfiguration configuration)
        {
            container.RegisterInstance(configuration.GetSection("WebServices").LoadConfiguration<WebServicesConfiguration>());
        }

        public static void AddWebServices(this IServiceCollection services, Container container)
        {
            services.AddHttpClient<IClientConfigurationService, ClientConfigurationService>();
            services.AddSingleton(provider => container.GetInstance<WebServicesConfiguration>().ProductConfigs);
        }
    }
}
