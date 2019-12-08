#region Usings

using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Eshva.DockerCompose.Exceptions;

#endregion


namespace Eshva.DockerCompose.Infrastructure
{
    public sealed class ExecutableProcessStarter : IProcessStarter
    {
        public ExecutableProcessStarter(string executableName)
        {
            _executableName = executableName;
        }

        public TextReader StandardOutput { get; private set; } = new StringReader(string.Empty);

        public TextReader StandardError { get; private set; } = new StringReader(string.Empty);

        public Task<int> Start(string arguments, TimeSpan executionTimeout)
        {
            var processStartInfo = new ProcessStartInfo(_executableName, arguments)
                                   {
                                       RedirectStandardOutput = true,
                                       RedirectStandardError = true,
                                       CreateNoWindow = false,
                                       UseShellExecute = false
                                   };
            Process process;
            try
            {
                process = Process.Start(processStartInfo);
            }
            catch (Exception exception)
            {
                throw new ProcessStartException($"An error occured during starting the executable '{_executableName}'.", exception);
            }

            if (process == null)
            {
                throw new InvalidOperationException("Process not started.");
            }

            try
            {
                var wasExitedNormally = process.WaitForExit((int)executionTimeout.TotalMilliseconds);
                if (!wasExitedNormally)
                {
                    throw new TimeoutException();
                }

                return Task.FromResult(process.ExitCode);
            }
            finally
            {
                StandardOutput = process.StandardOutput;
                StandardError = process.StandardError;
                process.Dispose();
            }
        }

        private readonly string _executableName;
    }
}
