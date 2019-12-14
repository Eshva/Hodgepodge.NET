namespace Eshva.DockerCompose.Commands.UpProject
{
    public sealed class UpProjectCommandBuilder : CommandBuilderBase<UpProjectCommand>
    {
        internal UpProjectCommandBuilder(UpProjectCommand command) : base(command)
        {
        }
    }

    public static class UpProjectCommandBuilderExtensions
    {
        public static UpProjectCommandBuilder Attached(this UpProjectCommandBuilder builder)
        {
            builder.Command.Attached = true;
            return builder;
        }

        public static UpProjectCommandBuilder WithQuietPull(this UpProjectCommandBuilder builder)
        {
            builder.Command.WithQuietPull = true;
            return builder;
        }

        public static UpProjectCommandBuilder DoNotStartLinkedServices(this UpProjectCommandBuilder builder)
        {
            builder.Command.DoNotStartLinkedServices = true;
            return builder;
        }

        public static UpProjectCommandBuilder ForceRecreateContainers(this UpProjectCommandBuilder builder)
        {
            builder.Command.ForceRecreateContainers = true;
            return builder;
        }

        public static UpProjectCommandBuilder RecreateDependedContainers(this UpProjectCommandBuilder builder)
        {
            builder.Command.RecreateDependedContainers = true;
            return builder;
        }

        public static UpProjectCommandBuilder DoNotRecreateExistingContainers(this UpProjectCommandBuilder builder)
        {
            builder.Command.DoNotRecreateExistingContainers = true;
            return builder;
        }

        public static UpProjectCommandBuilder DoNotBuildMissingImages(this UpProjectCommandBuilder builder)
        {
            builder.Command.DoNotBuildMissingImages = true;
            return builder;
        }

        public static UpProjectCommandBuilder DoNotStartServices(this UpProjectCommandBuilder builder)
        {
            builder.Command.DoNotStartServices = true;
            return builder;
        }

        public static UpProjectCommandBuilder ForceBuildImages(this UpProjectCommandBuilder builder)
        {
            builder.Command.ForceBuildImages = true;
            return builder;
        }

        public static UpProjectCommandBuilder StopAllContainersIfAnyOneStopped(this UpProjectCommandBuilder builder)
        {
            builder.Command.StopAllContainersIfAnyOneStopped = true;
            return builder;
        }

        public static UpProjectCommandBuilder RemoveOrphanContainers(this UpProjectCommandBuilder builder)
        {
            builder.Command.RemoveOrphanContainers = true;
            return builder;
        }

        public static UpProjectCommandBuilder RecreateAnonymousVolumes(this UpProjectCommandBuilder builder)
        {
            builder.Command.RecreateAnonymousVolumes = true;
            return builder;
        }

        public static UpProjectCommandBuilder ShutdownTimeoutSeconds(this UpProjectCommandBuilder builder, int shutdownTimeoutSeconds)
        {
            builder.Command.ShutdownTimeoutSeconds = shutdownTimeoutSeconds;
            return builder;
        }

        public static UpProjectCommandBuilder TakeExitCodeFromService(this UpProjectCommandBuilder builder, string serviceName)
        {
            builder.Command.TakeExitCodeFromService = serviceName;
            return builder;
        }
        public static UpProjectCommandBuilder ScaleService(this UpProjectCommandBuilder builder, string serviceName, int instanceNumber)
        {
            builder.Command.Scaling.Add(serviceName, instanceNumber);
            return builder;
        }
    }
}
