#region Usings

using System.Net.Http;
using System.Threading.Tasks;
using Eshva.Common.WebApp.WebService;
using Eshva.Polls.Configuration.Contracts.Client;
using JetBrains.Annotations;

#endregion


namespace Eshva.Polls.Configuration.WebService.Client
{
    [UsedImplicitly]
    public sealed class ClientConfigurationService : TypedHttpClientServiceBase, IClientConfigurationService
    {
        public ClientConfigurationService(HttpClient httpClient, ProductConfigurationWebServiceConfiguration configuration) : base(httpClient)
        {
            HttpClient.BaseAddress = configuration.BaseUri;
        }

        public async Task SetClientConfiguration(ClientConfiguration clientConfiguration)
        {
            // TODO: Catch exceptions of HttpClient.PostAsync.
            var response = await HttpClient.PostAsync(ClientConfigs, MakeJsonContent(clientConfiguration));
            if (!response.IsSuccessStatusCode)
            {
                throw new ExternalServiceException("Couldn't set client configuration.");
            }
        }

        private const string ClientConfigs = "client";
    }
}
