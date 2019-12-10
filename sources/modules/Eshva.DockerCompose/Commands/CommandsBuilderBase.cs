#region Usings

using System.Linq;
using Eshva.DockerCompose.Exceptions;

#endregion


namespace Eshva.DockerCompose.Commands
{
    public class CommandsBuilderBase<TCommand>
        where TCommand : CommandBase
    {
        protected CommandsBuilderBase(TCommand command)
        {
            Command = command;
        }

        public TCommand Build()
        {
            var errors = Command.Verify();
            if (errors.Any())
            {
                throw CommandBuildException.Create(typeof(TCommand).Name, errors);
            }

            return Command;
        }

        internal TCommand Command { get; }
    }
}
