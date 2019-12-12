#region Usings

using System;
using System.Linq;
using System.Threading.Tasks;
using Eshva.DockerCompose.Exceptions;
using Eshva.DockerCompose.Infrastructure;

#endregion


namespace Eshva.DockerCompose.Commands
{
    public abstract class CommandBase
    {
        protected CommandBase(IProcessStarter starter, params string[] files)
        {
            _starter = starter;
            _files = files;
        }

        protected CommandBase(params string[] files)
            : this(new ExecutableProcessStarter(DockerComposeExecutable), files)
        {
            _files = files;
        }

        public Task Execute() => Execute(_oneDayLong);

        public async Task Execute(TimeSpan executionTimeout)
        {
            var projectFileNames = _files.Aggregate(string.Empty, (result, current) => $"{result} -f \"{current}\"");
            var arguments = string.Join(" ", PrepareArguments());

            var exitCode = await _starter.Start(
                $"{Command.Trim()} {projectFileNames.Trim()} {arguments.Trim()}".Trim(),
                executionTimeout);
            if (exitCode != 0)
            {
                throw new CommandExecutionException(
                    $"Docker Compose command {GetType().Name} executed with an error. " +
                    $"Exit code was {exitCode}.{Environment.NewLine}{Environment.NewLine}" +
                    $"Command STDOUT:{Environment.NewLine}{_starter.StandardOutput}{Environment.NewLine}" +
                    $"Command STDERR:{Environment.NewLine}{_starter.StandardError}{Environment.NewLine}");
            }
        }

        protected internal abstract string[] Verify();

        protected abstract string Command { get; }

        protected abstract string[] PrepareArguments();

        private const string DockerComposeExecutable = "docker-compose";
        private readonly string[] _files;
        private readonly TimeSpan _oneDayLong = TimeSpan.FromDays(1);

        private readonly IProcessStarter _starter;
    }
}
