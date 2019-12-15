#region Usings

using System.Collections.Generic;
using Eshva.DockerCompose.Extensions;
using Eshva.DockerCompose.Infrastructure;
using FluentValidation;

#endregion


namespace Eshva.DockerCompose.Commands.StartServices
{
    /// <summary>
    /// Starts existing containers for services.
    /// </summary>
    public sealed class StartServicesCommand : CommandBase
    {
        /// <inheritdoc cref="CommandBase"/>
        private StartServicesCommand(IProcessStarter starter, params string[] files) : base(starter, files)
        {
        }

        /// <inheritdoc cref="CommandBase"/>
        private StartServicesCommand(params string[] files) : base(files)
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
        public static StartServicesCommandBuilder WithFiles(params string[] files) =>
            new StartServicesCommandBuilder(new StartServicesCommand(files));

        /// <summary>
        /// Creates a command with specified in <paramref name="files"/> files with process starter <paramref name="starter"/>.
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
        public static StartServicesCommandBuilder WithFilesAndStarter(IProcessStarter starter, params string[] files) =>
            new StartServicesCommandBuilder(new StartServicesCommand(starter, files));

        internal bool DoStartAllServices { get; set; }

        internal List<string> Services { get; } = new List<string>();

        /// <inheritdoc cref="CommandBase.CreateValidator"/>
        protected internal override IValidator CreateValidator() => new StartServicesCommandValidator();

        /// <inheritdoc cref="CommandBase.Command"/>
        protected override string Command { get; } = "start";

        /// <inheritdoc cref="CommandBase.PrepareArguments"/>
        protected override string[] PrepareArguments()
        {
            var arguments = new List<string>();
            var services = string.Join(" ", Services);
            arguments.AddConditionally(!string.IsNullOrWhiteSpace(services), services);
            return arguments.ToArray();
        }
    }
}
