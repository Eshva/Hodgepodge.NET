namespace Eshva.DockerCompose.Commands.UpProject
{
    /// <summary>
    /// The builder for <see cref="UpProjectCommand"/> command.
    /// </summary>
    public sealed class UpProjectCommandBuilder : CommandBuilderBase<UpProjectCommand>
    {
        /// <inheritdoc cref="CommandBuilderBase{TCommand}"/>
        internal UpProjectCommandBuilder(UpProjectCommand command) : base(command)
        {
        }

        public UpProjectCommandBuilder Attached()
        {
            Command.Attached = true;
            return this;
        }

        public UpProjectCommandBuilder WithQuietPull()
        {
            Command.WithQuietPull = true;
            return this;
        }

        public UpProjectCommandBuilder DoNotStartLinkedServices()
        {
            Command.DoNotStartLinkedServices = true;
            return this;
        }

        public UpProjectCommandBuilder ForceRecreateContainers()
        {
            Command.ForceRecreateContainers = true;
            return this;
        }

        public UpProjectCommandBuilder RecreateDependedContainers()
        {
            Command.RecreateDependedContainers = true;
            return this;
        }

        public UpProjectCommandBuilder DoNotRecreateExistingContainers()
        {
            Command.DoNotRecreateExistingContainers = true;
            return this;
        }

        public UpProjectCommandBuilder DoNotBuildMissingImages()
        {
            Command.DoNotBuildMissingImages = true;
            return this;
        }

        public UpProjectCommandBuilder DoNotStartServices()
        {
            Command.DoNotStartServices = true;
            return this;
        }

        public UpProjectCommandBuilder ForceBuildImages()
        {
            Command.ForceBuildImages = true;
            return this;
        }

        public UpProjectCommandBuilder StopAllContainersIfAnyOneStopped()
        {
            Command.StopAllContainersIfAnyOneStopped = true;
            return this;
        }

        public UpProjectCommandBuilder RemoveOrphanContainers()
        {
            Command.RemoveOrphanContainers = true;
            return this;
        }

        public UpProjectCommandBuilder RecreateAnonymousVolumes()
        {
            Command.RecreateAnonymousVolumes = true;
            return this;
        }

        public UpProjectCommandBuilder ShutdownTimeoutSeconds(int shutdownTimeoutSeconds)
        {
            Command.ShutdownTimeoutSeconds = shutdownTimeoutSeconds;
            return this;
        }

        public UpProjectCommandBuilder TakeExitCodeFromService(string serviceName)
        {
            Command.TakeExitCodeFromService = serviceName;
            return this;
        }

        public UpProjectCommandBuilder ScaleService(string serviceName, int instanceNumber)
        {
            Command.Scaling.Add(serviceName, instanceNumber);
            return this;
        }
    }
}
