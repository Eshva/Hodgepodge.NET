namespace Eshva.DockerCompose.Commands.DownProject
{
    public sealed class DownProjectCommandBuilder : CommandBuilderBase<DownProjectCommand>
    {
        internal DownProjectCommandBuilder(DownProjectCommand command) : base(command)
        {
        }
    }

    public static class DownProjectCommandBuilderExtensions
    {
        public static DownProjectCommandBuilder RemoveAllImages(this DownProjectCommandBuilder builder)
        {
            builder.Command.RemoveImages = DownProjectCommand.ImageRemovingType.All;
            return builder;
        }

        public static DownProjectCommandBuilder RemoveLocalImages(this DownProjectCommandBuilder builder)
        {
            builder.Command.RemoveImages = DownProjectCommand.ImageRemovingType.Local;
            return builder;
        }

        public static DownProjectCommandBuilder RemoveVolumes(this DownProjectCommandBuilder builder)
        {
            builder.Command.RemoveVolumes = true;
            return builder;
        }

        public static DownProjectCommandBuilder RemoveOrphanContainers(this DownProjectCommandBuilder builder)
        {
            builder.Command.RemoveOrphanContainers = true;
            return builder;
        }

        public static DownProjectCommandBuilder ShutdownTimeoutSeconds(this DownProjectCommandBuilder builder, int shutdownTimeoutSeconds)
        {
            builder.Command.ShutdownTimeoutSeconds = shutdownTimeoutSeconds;
            return builder;
        }
    }
}
