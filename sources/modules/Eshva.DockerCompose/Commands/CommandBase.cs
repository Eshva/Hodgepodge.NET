#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eshva.DockerCompose.Exceptions;
using Eshva.DockerCompose.Infrastructure;

#endregion


namespace Eshva.DockerCompose.Commands
{
    public abstract class CommandBase
    {
        protected CommandBase(IProcessStarter processStarter, params string[] projectFileNames)
        {
            _processStarter = processStarter;
            _projectFileNames = projectFileNames;
        }

        protected CommandBase(params string[] projectFileNames)
            : this(new ExecutableProcessStarter(DockerComposeExecutable), projectFileNames)
        {
        }

        public Task Execute() => Execute(_oneDayLong);

        public async Task Execute(TimeSpan executionTimeout)
        {
            var projectFileNames = _projectFileNames.Aggregate(string.Empty, (result, current) => $"{result} -f \"{current}\"");
            var arguments = PrepareArguments().Aggregate(string.Empty, (result, current) => $"{result} {current}");

            var exitCode = await _processStarter.Start($"{projectFileNames.Trim()} {arguments.Trim()}".Trim(), executionTimeout);
            if (exitCode != 0)
            {
                throw new CommandExecutionException(
                    $"Docker Compose command {GetType().Name} executed with an error. " +
                    $"Exit code was {exitCode}.{Environment.NewLine}{Environment.NewLine}" +
                    $"Command STDOUT:{Environment.NewLine}{_processStarter.StandardOutput}{Environment.NewLine}" +
                    $"Command STDERR:{Environment.NewLine}{_processStarter.StandardError}{Environment.NewLine}");
            }
        }

        protected abstract IReadOnlyCollection<string> PrepareArguments();

        private const string DockerComposeExecutable = "docker-compose";
        private readonly TimeSpan _oneDayLong = TimeSpan.FromDays(1);
        private readonly IProcessStarter _processStarter;
        private readonly string[] _projectFileNames;
    }
}
