#region Usings

using System.Collections.Generic;
using Eshva.DockerCompose.Extensions;
using Eshva.DockerCompose.Infrastructure;

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

        internal bool Attached { private get; set; } = Default.Attached;

        internal bool WithQuietPull { private get; set; } = Default.WithQuietPull;

        internal bool DoNotStartLinkedServices { private get; set; } = Default.DoNotStartLinkedServices;

        internal bool ForceRecreateContainers { private get; set; } = Default.ForceRecreateContainers;

        internal bool DoNotRecreateExistingContainers { private get; set; } = Default.DoNotRecreateExistingContainers;

        internal bool DoNotStartServices { private get; set; } = Default.DoNotStartServices;

        internal bool RecreateDependedContainers { private get; set; } = Default.RecreateDependedContainers;

        internal bool DoNotBuildMissingImages { private get; set; } = Default.DoNotBuildMissingImages;

        internal bool ForceBuildImages { private get; set; } = Default.ForceBuildImages;

        internal bool StopAllContainersIfAnyOneStopped { private get; set; } = Default.StopAllContainersIfAnyOneStopped;

        internal bool RecreateAnonymousVolumes { private get; set; } = Default.RecreateAnonymousVolumes;

        internal bool RemoveOrphanContainers { private get; set; } = Default.RemoveOrphanContainers;

        internal int ShutdownTimeoutSeconds { private get; set; } = Default.ShutdownTimeoutSeconds;

        internal string TakeExitCodeFromService { private get; set; } = Default.TakeExitCodeFromService;

        internal IDictionary<string, int> Scaling { get; } = new Dictionary<string, int>();

        protected internal override string[] Verify()
        {
            // TODO: Verification.
            return new string[] { };
        }

        protected override string Command => "up";

        protected override string[] PrepareArguments()
        {
            var arguments = new List<string>();
            arguments.AddConditionally(!Attached, "--detach");
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
