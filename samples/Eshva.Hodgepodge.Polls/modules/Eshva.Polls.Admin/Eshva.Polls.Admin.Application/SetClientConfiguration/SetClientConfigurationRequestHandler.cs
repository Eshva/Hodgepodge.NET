#region Usings

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Eshva.Polls.Configuration.Contracts.Client;
using JetBrains.Annotations;
using MediatR;

#endregion


namespace Eshva.Polls.Admin.Application.SetClientConfiguration
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public sealed class SetClientConfigurationRequestHandler : IRequestHandler<SetClientConfigurationRequest, Unit>
    {
        public SetClientConfigurationRequestHandler(IClientConfigurationService clientConfigurationService)
        {
            _clientConfigurationService = clientConfigurationService;
        }

        public async Task<Unit> Handle(SetClientConfigurationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _clientConfigurationService.SetClientConfiguration(
                    new ClientConfiguration
                    {
                        // TODO: Replace this mapping with an AutoMapper mapping.
                        ConfigurationRefreshIntervalSeconds = request.ConfigurationRefreshIntervalSeconds
                    });
            }
            catch (HttpRequestException exception)
            {
                // TODO: Replace ApplicationException with more specialized exception type.
                throw new ApplicationException("An error occured during an external service call.", exception);
            }

            return Unit.Value;
        }

        private readonly IClientConfigurationService _clientConfigurationService;
    }
}
