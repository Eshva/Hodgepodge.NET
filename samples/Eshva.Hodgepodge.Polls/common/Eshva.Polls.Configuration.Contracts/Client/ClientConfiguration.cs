#region Usings

using JetBrains.Annotations;

#endregion


namespace Eshva.Polls.Configuration.Contracts.Client
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public sealed class ClientConfiguration
    {
        public int ConfigurationRefreshIntervalSeconds { get; set; }
    }
}
