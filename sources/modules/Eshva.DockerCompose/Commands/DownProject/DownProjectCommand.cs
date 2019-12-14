#region Usings

using System.Collections.Generic;
using Eshva.DockerCompose.Infrastructure;
using FluentValidation;

#endregion


namespace Eshva.DockerCompose.Commands.DownProject
{
    public sealed class DownProjectCommand : CommandBase
    {
        // TODO: Private constructors and a builder.
        public DownProjectCommand(IProcessStarter starter, params string[] files) : base(starter, files)
        {
        }

        public DownProjectCommand(params string[] files) : base(files)
        {
        }

        protected internal override IValidator CreateValidator()
        {
            // TODO: Implement DownProjectCommandValidator
            return new InlineValidator<DownProjectCommand>();
        }

        protected override string Command => "down";

        protected override string[] PrepareArguments()
        {
            var arguments = new List<string>();
            return arguments.ToArray();
        }
    }
}
