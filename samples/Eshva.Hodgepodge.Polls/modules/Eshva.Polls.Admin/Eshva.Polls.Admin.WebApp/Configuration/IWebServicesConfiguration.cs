#region Usings

using Eshva.Polls.Configuration.WebService.Client;

#endregion


namespace Eshva.Polls.Admin.WebApp.Configuration
{
    public interface IWebServicesConfiguration
    {
        ProductConfigurationWebServiceConfiguration ProductConfigs { get; }
    }
}
