#region Usings

using JetBrains.Annotations;

#endregion


namespace Eshva.Polls.Admin.Application.ClientConfiguration
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public sealed class ClientConfiguration
    {
        public int ConfigurationRefreshIntervalSeconds { get; set; }
    }
}
