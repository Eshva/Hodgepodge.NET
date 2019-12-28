#region Usings

using System.Threading;
using System.Threading.Tasks;

#endregion


namespace Eshva.Common.WebApp.Readiness
{
    public interface IReadinessTester
    {
        Task<bool> IsReady(CancellationToken cancellationToken);
    }
}
