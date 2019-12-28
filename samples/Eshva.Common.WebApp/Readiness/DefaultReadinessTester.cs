#region Usings

using System.Threading;
using System.Threading.Tasks;

#endregion


namespace Eshva.Common.WebApp.Readiness
{
    public sealed class DefaultReadinessTester : IReadinessTester
    {
        public Task<bool> IsReady(CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }
}
