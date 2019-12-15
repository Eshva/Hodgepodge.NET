#region Usings

using System.Collections.Generic;
using Eshva.DockerCompose.Commands.StartServices;
using Eshva.DockerCompose.Extensions;
using Eshva.DockerCompose.Infrastructure;
using FluentValidation;

#endregion


namespace Eshva.DockerCompose.Commands.StopServices
{
    /// <summary>
    /// Stops running containers without removing them. They can be started again with <see cref="StartServicesCommand"/>.
    /// </summary>
    public sealed class StopServicesCommand : CommandBase
    {
        /// <inheritdoc cref="CommandBase"/>
        private StopServicesCommand(IProcessStarter starter, params string[] files) : base(starter, files)
        {
        }

        /// <inheritdoc cref="CommandBase"/>
        private StopServicesCommand(params string[] files) : base(files)
        {
        }

        /// <summary>
        /// Creates a command with specified in <paramref name="files"/> files.
        /// </summary>
        /// <param name="files">
        /// Project files.
        /// </param>
        /// <returns>
        /// Command builder.
        /// </returns>
        public static StopServicesCommandBuilder WithFiles(params string[] files) =>
            new StopServicesCommandBuilder(new StopServicesCommand(files));

        /// <summary>
        /// Creates a command with specified in <paramref name="files"/> files with process <paramref name="starter"/>.
        /// </summary>
        /// <param name="starter">
        /// Process starter that will be used to start docker-compose executable.
        /// </param>
        /// <param name="files">
        /// Project files.
        /// </param>
        /// <returns>
        /// Command builder.
        /// </returns>
        public static StopServicesCommandBuilder WithFilesAndStarter(IProcessStarter starter, params string[] files) =>
            new StopServicesCommandBuilder(new StopServicesCommand(starter, files));

        internal bool DoStopAllServices { get; set; }

        internal List<string> Services { get; } = new List<string>();

        /// <inheritdoc cref="CommandBase.CreateValidator"/>
        protected internal override IValidator CreateValidator() => new StopServicesCommandValidator();

        internal int ShutdownTimeoutSeconds { get; set; } = Default.ShutdownTimeoutSeconds;

        /// <inheritdoc cref="CommandBase.Command"/>
        protected override string Command { get; } = "stop";

        /// <inheritdoc cref="CommandBase.PrepareArguments"/>
        protected override string[] PrepareArguments()
        {
            var arguments = new List<string>();
            var services = string.Join(" ", Services);
            arguments.AddConditionally(
                ShutdownTimeoutSeconds != Default.ShutdownTimeoutSeconds,
                $"--timeout {ShutdownTimeoutSeconds}");
            arguments.AddConditionally(!string.IsNullOrWhiteSpace(services), services);
            return arguments.ToArray();
        }

        private static class Default
        {
            public const int ShutdownTimeoutSeconds = 10;
        }
    }
}
