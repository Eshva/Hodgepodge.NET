#region Usings

using System.Collections.Generic;
using System.Linq;
using Eshva.DockerCompose.Extensions;
using Eshva.DockerCompose.Infrastructure;
using FluentValidation;

#endregion


namespace Eshva.DockerCompose.Commands.UpProject
{
    public sealed class UpProjectCommand : CommandBase
    {
        private UpProjectCommand(IProcessStarter processStarter, params string[] files) : base(processStarter, files)
        {
        }

        private UpProjectCommand(params string[] files) : base(files)
        {
        }

        public static UpProjectCommandBuilder WithFiles(params string[] files) =>
            new UpProjectCommandBuilder(new UpProjectCommand(files));

        public static UpProjectCommandBuilder WithFilesAndStarter(IProcessStarter starter, params string[] files) =>
            new UpProjectCommandBuilder(new UpProjectCommand(starter, files));

        internal bool Attached { get; set; } = Default.Attached;

        internal bool WithQuietPull { private get; set; } = Default.WithQuietPull;

        internal bool DoNotStartLinkedServices { private get; set; } = Default.DoNotStartLinkedServices;

        internal bool ForceRecreateContainers { private get; set; } = Default.ForceRecreateContainers;

        internal bool RecreateDependedContainers { private get; set; } = Default.RecreateDependedContainers;

        internal bool DoNotRecreateExistingContainers { private get; set; } = Default.DoNotRecreateExistingContainers;

        internal bool DoNotBuildMissingImages { private get; set; } = Default.DoNotBuildMissingImages;

        internal bool DoNotStartServices { private get; set; } = Default.DoNotStartServices;

        internal bool ForceBuildImages { private get; set; } = Default.ForceBuildImages;

        internal bool StopAllContainersIfAnyOneStopped { get; set; } = Default.StopAllContainersIfAnyOneStopped;

        internal bool RecreateAnonymousVolumes { private get; set; } = Default.RecreateAnonymousVolumes;

        internal bool RemoveOrphanContainers { private get; set; } = Default.RemoveOrphanContainers;

        internal int ShutdownTimeoutSeconds { private get; set; } = Default.ShutdownTimeoutSeconds;

        internal string TakeExitCodeFromService { private get; set; } = Default.TakeExitCodeFromService;

        internal IDictionary<string, int> Scaling { get; } = new Dictionary<string, int>();

        protected internal override IValidator CreateValidator() => new UpProjectCommandValidator();

        protected override string Command => "up";

        protected override string[] PrepareArguments()
        {
            var arguments = new List<string>();
            arguments.AddConditionally(
                Attached == Default.Attached,
                "--detach");
            arguments.AddConditionally(
                WithQuietPull != Default.WithQuietPull,
                "--quiet-pull");
            arguments.AddConditionally(
                DoNotStartLinkedServices != Default.DoNotStartLinkedServices,
                "--no-deps");
            arguments.AddConditionally(
                ForceRecreateContainers != Default.ForceRecreateContainers,
                "--force-recreate");
            arguments.AddConditionally(
                RecreateDependedContainers != Default.RecreateDependedContainers,
                "--always-recreate-deps");
            arguments.AddConditionally(
                DoNotRecreateExistingContainers != Default.DoNotRecreateExistingContainers,
                "--no-recreate");
            arguments.AddConditionally(
                DoNotBuildMissingImages != Default.DoNotBuildMissingImages,
                "--no-build");
            arguments.AddConditionally(
                DoNotStartServices != Default.DoNotStartServices,
                "--no-start");
            arguments.AddConditionally(
                ForceBuildImages != Default.ForceBuildImages,
                "--build");
            arguments.AddConditionally(
                StopAllContainersIfAnyOneStopped != Default.StopAllContainersIfAnyOneStopped,
                "--abort-on-container-exit");
            arguments.AddConditionally(
                RecreateAnonymousVolumes != Default.RecreateAnonymousVolumes,
                "--renew-anon-volumes");
            arguments.AddConditionally(
                RemoveOrphanContainers != Default.RemoveOrphanContainers,
                "--remove-orphans");
            arguments.AddConditionally(
                ShutdownTimeoutSeconds != Default.ShutdownTimeoutSeconds,
                $"--timeout {ShutdownTimeoutSeconds}");
            arguments.AddConditionally(
                TakeExitCodeFromService != Default.TakeExitCodeFromService,
                $"--exit-code-from {TakeExitCodeFromService}");
            arguments.AddConditionally(
                Scaling.Count > 0,
                string.Join(" ", Scaling.Select(pair => $"--scale {pair.Key}={pair.Value}")));
            return arguments.ToArray();
        }

        private static class Default
        {
            public const bool Attached = false;
            public const bool WithQuietPull = false;
            public const bool DoNotStartLinkedServices = false;
            public const bool ForceRecreateContainers = false;
            public const bool DoNotRecreateExistingContainers = false;
            public const bool DoNotStartServices = false;
            public const bool DoNotBuildMissingImages = false;
            public const bool RecreateDependedContainers = false;
            public const bool ForceBuildImages = false;
            public const bool StopAllContainersIfAnyOneStopped = false;
            public const bool RecreateAnonymousVolumes = false;
            public const bool RemoveOrphanContainers = false;
            public const int ShutdownTimeoutSeconds = 10;
            public const string TakeExitCodeFromService = "";
        }
    }
}
