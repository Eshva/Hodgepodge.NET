namespace Eshva.DockerCompose.Exceptions
{
    public sealed class CommandExecutionException : DockerComposeException
    {
        public CommandExecutionException(string message) : base(message)
        {
        }
    }
}
