namespace Eshva.DockerCompose.Commands.UpProject
{
    public sealed class UpProjectCommandBuilder : CommandsBuilderBase<UpProjectCommand>
    {
        internal UpProjectCommandBuilder(UpProjectCommand command) : base(command)
        {
        }
    }

    public static class UpProjectCommandBuilderExtensions
    {
        public static UpProjectCommandBuilder Detached(this UpProjectCommandBuilder builder)
        {
            builder.Command.IsDetached = true;
            return builder;
        }

        public static UpProjectCommandBuilder Attached(this UpProjectCommandBuilder builder)
        {
            builder.Command.IsDetached = false;
            return builder;
        }
    }
}
