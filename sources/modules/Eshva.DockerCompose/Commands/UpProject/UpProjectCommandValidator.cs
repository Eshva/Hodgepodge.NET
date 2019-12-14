#region Usings

using FluentValidation;

#endregion


namespace Eshva.DockerCompose.Commands.UpProject
{
    public sealed class UpProjectCommandValidator : AbstractValidator<UpProjectCommand>
    {
        public UpProjectCommandValidator()
        {
            RuleFor(command => command)
                .Must(command => !(command.StopAllContainersIfAnyOneStopped && !command.Attached))
                .WithMessage("UP command can't stop all containers if any of them stopped and the command executed in detached mode.");
        }
    }
}
