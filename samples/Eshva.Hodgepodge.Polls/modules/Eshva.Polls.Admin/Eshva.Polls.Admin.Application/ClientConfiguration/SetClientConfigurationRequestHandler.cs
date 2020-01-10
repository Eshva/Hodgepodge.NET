#region Usings

using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;

#endregion


namespace Eshva.Polls.Admin.Application.ClientConfiguration
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public sealed class SetClientConfigurationRequestHandler : IRequestHandler<SetClientConfigurationRequest, Unit>
    {
        public SetClientConfigurationRequestHandler(IClientConfigurationService clientConfigurationService)
        {
        }

        public Task<Unit> Handle(SetClientConfigurationRequest request, CancellationToken cancellationToken)
        {
            return null;
        }
    }
}
