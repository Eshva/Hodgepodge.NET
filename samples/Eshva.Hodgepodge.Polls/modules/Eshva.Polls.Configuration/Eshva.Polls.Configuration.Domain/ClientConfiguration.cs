#region Usings

using JetBrains.Annotations;

#endregion


namespace Eshva.Polls.Configuration.Domain
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public sealed class ClientConfiguration
    {
        public int ConfigurationRefreshIntervalSeconds { get; set; }
    }
}
