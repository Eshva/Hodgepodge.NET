#region Usings

using System;
using System.IO;
using System.Threading.Tasks;

#endregion


namespace Eshva.DockerCompose.Infrastructure
{
    public interface IProcessStarter
    {
        TextReader StandardOutput { get; }

        TextReader StandardError { get; }

        Task<int> Start(string arguments, TimeSpan executionTimeout);
    }
}
