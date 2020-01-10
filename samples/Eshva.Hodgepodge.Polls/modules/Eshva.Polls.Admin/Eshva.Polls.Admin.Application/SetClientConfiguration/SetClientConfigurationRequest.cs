#region Usings

using JetBrains.Annotations;
using MediatR;

#endregion


namespace Eshva.Polls.Admin.Application.SetClientConfiguration
{
    [UsedImplicitly]
    public sealed class SetClientConfigurationRequest : IRequest<Unit>
    {
        public int ConfigurationRefreshIntervalSeconds { get; set; }
    }
}
