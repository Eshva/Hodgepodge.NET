#region Usings

using System.Threading.Tasks;
using MediatR;

#endregion


namespace Eshva.Polls.Admin.Application.ClientConfiguration
{
    public interface IClientConfigurationService
    {
        Task<Unit> SetClientConfiguration(ClientConfiguration clientConfiguration);
    }
}
