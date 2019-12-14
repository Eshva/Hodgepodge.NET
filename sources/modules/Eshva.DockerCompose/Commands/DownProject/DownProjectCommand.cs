#region Usings

using System.Collections.Generic;
using Eshva.DockerCompose.Extensions;
using Eshva.DockerCompose.Infrastructure;
using FluentValidation;

#endregion


namespace Eshva.DockerCompose.Commands.DownProject
{
    public sealed class DownProjectCommand : CommandBase
    {
        private DownProjectCommand(IProcessStarter starter, params string[] files) : base(starter, files)
        {
        }

        private DownProjectCommand(params string[] files) : base(files)
        {
        }

        public static DownProjectCommandBuilder WithFiles(params string[] files) =>
            new DownProjectCommandBuilder(new DownProjectCommand(files));

        public static DownProjectCommandBuilder WithFilesAndStarter(IProcessStarter starter, params string[] files) =>
            new DownProjectCommandBuilder(new DownProjectCommand(starter, files));

        protected internal override IValidator CreateValidator()
        {
            // TODO: Implement DownProjectCommandValidator
            return new InlineValidator<DownProjectCommand>();
        }

        internal ImageRemovingType RemoveImages { get; set; } = Default.RemoveImages;

        internal bool RemoveVolumes { get; set; } = Default.RemoveVolumes;

        internal bool RemoveOrphanContainers { get; set; } = Default.RemoveOrphanContainers;

        internal int ShutdownTimeoutSeconds { get; set; } = Default.ShutdownTimeoutSeconds;

        protected override string Command => "down";

        protected override string[] PrepareArguments()
        {
            var arguments = new List<string>();
            arguments.AddConditionally(
                RemoveImages == ImageRemovingType.All,
                "--rmi all");
            arguments.AddConditionally(
                RemoveImages == ImageRemovingType.Local,
                "--rmi local");
            arguments.AddConditionally(
                RemoveVolumes != Default.RemoveVolumes,
                "--volumes");
            arguments.AddConditionally(
                RemoveOrphanContainers != Default.RemoveOrphanContainers,
                "--remove-orphans");
            arguments.AddConditionally(
                ShutdownTimeoutSeconds != Default.ShutdownTimeoutSeconds,
                $"--timeout {ShutdownTimeoutSeconds}");
            return arguments.ToArray();
        }

        private static class Default
        {
            public const ImageRemovingType RemoveImages = ImageRemovingType.None;
            public const bool RemoveVolumes = false;
            public const bool RemoveOrphanContainers = false;
            public const int ShutdownTimeoutSeconds = 10;
        }

        internal enum ImageRemovingType
        {
            None,
            All,
            Local
        }
    }
}
