#region Usings

using System.Threading.Tasks;

#endregion


namespace Eshva.Polls.Configuration.Contracts.Client
{
    public interface IClientConfigurationService
    {
        Task SetClientConfiguration(ClientConfiguration clientConfiguration);
    }
}
