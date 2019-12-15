#region Usings

using System.Collections.Generic;
using Eshva.DockerCompose.Extensions;
using Eshva.DockerCompose.Infrastructure;
using FluentValidation;

#endregion


namespace Eshva.DockerCompose.Commands.Logs
{
    /// <summary>
    /// Takes log output from services in project.
    /// </summary>
    public sealed class LogsCommand : CommandBase
    {
        /// <inheritdoc cref="CommandBase"/>
        private LogsCommand(IProcessStarter starter, params string[] files) : base(starter, files)
        {
        }

        /// <inheritdoc cref="CommandBase"/>
        private LogsCommand(params string[] files) : base(files)
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
        public static LogsCommandBuilder WithFiles(params string[] files) =>
            new LogsCommandBuilder(new LogsCommand(files));

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
        public static LogsCommandBuilder WithFilesAndStarter(IProcessStarter starter, params string[] files) =>
            new LogsCommandBuilder(new LogsCommand(starter, files));

        internal List<string> FromServices { get; } = new List<string>();

        internal bool WithTimestamps { get; set; } = Default.WithTimestamps;

        internal int TakeNumberOfLines { get; set; } = Default.TakeNumberOfLines;

        internal bool DoTakeFromAllServices { get; set; }

        /// <inheritdoc cref="CommandBase.CreateValidator"/>
        protected internal override IValidator CreateValidator() => new LogsCommandValidator();

        /// <inheritdoc cref="CommandBase.Command"/>
        protected override string Command { get; } = "logs";

        /// <inheritdoc cref="CommandBase.PrepareArguments"/>
        protected override string[] PrepareArguments()
        {
            var arguments = new List<string>();
            arguments.AddConditionally(WithTimestamps, "--timestamps");
            arguments.AddConditionally(TakeNumberOfLines > 0, $"--tail {TakeNumberOfLines}");
            var services = string.Join(" ", FromServices);
            arguments.AddConditionally(!string.IsNullOrWhiteSpace(services), services);
            return arguments.ToArray();
        }

        private static class Default
        {
            public const bool WithTimestamps = false;
            public const int TakeNumberOfLines = 0;
        }
    }
}
