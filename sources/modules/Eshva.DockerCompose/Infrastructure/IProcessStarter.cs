#region Usings

using System;
using System.Threading.Tasks;

#endregion


namespace Eshva.DockerCompose.Infrastructure
{
    public interface IProcessStarter
    {
        Task<int> Start(string arguments, TimeSpan executionTimeout);
    }
}
