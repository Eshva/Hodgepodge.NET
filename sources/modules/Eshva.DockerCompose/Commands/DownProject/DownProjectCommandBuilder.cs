namespace Eshva.DockerCompose.Commands.DownProject
{
    public sealed class DownProjectCommandBuilder : CommandBuilderBase<DownProjectCommand>
    {
        internal DownProjectCommandBuilder(DownProjectCommand command) : base(command)
        {
        }

        public DownProjectCommandBuilder RemoveAllImages()
        {
            Command.RemoveImages = DownProjectCommand.ImageRemovingType.All;
            return this;
        }

        public DownProjectCommandBuilder RemoveLocalImages()
        {
            Command.RemoveImages = DownProjectCommand.ImageRemovingType.Local;
            return this;
        }

        public DownProjectCommandBuilder RemoveVolumes()
        {
            Command.RemoveVolumes = true;
            return this;
        }

        public DownProjectCommandBuilder RemoveOrphanContainers()
        {
            Command.RemoveOrphanContainers = true;
            return this;
        }

        public DownProjectCommandBuilder ShutdownTimeoutSeconds(int shutdownTimeoutSeconds)
        {
            Command.ShutdownTimeoutSeconds = shutdownTimeoutSeconds;
            return this;
        }
    }
}
