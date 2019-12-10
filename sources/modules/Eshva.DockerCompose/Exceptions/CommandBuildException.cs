#region Usings

using System;

#endregion


namespace Eshva.DockerCompose.Exceptions
{
    public sealed class CommandBuildException : DockerComposeException
    {
        private CommandBuildException(string message) : base(message)
        {
        }

        public static CommandBuildException Create(string commandType, string[] errors)
        {
            var message = $"During building {commandType} following errors occured:{Environment.NewLine}" +
                          $"{string.Join(Environment.NewLine, errors)}";
            return new CommandBuildException(message);
        }
    }
}
