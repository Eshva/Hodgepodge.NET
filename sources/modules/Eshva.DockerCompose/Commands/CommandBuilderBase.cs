#region Usings

using System.Linq;
using Eshva.DockerCompose.Exceptions;

#endregion


namespace Eshva.DockerCompose.Commands
{
    public class CommandBuilderBase<TCommand>
        where TCommand : CommandBase
    {
        protected CommandBuilderBase(TCommand command)
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
