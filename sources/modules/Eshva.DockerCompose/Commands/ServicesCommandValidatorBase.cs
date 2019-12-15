#region Usings

using System.Linq;
using FluentValidation;

#endregion


namespace Eshva.DockerCompose.Commands
{
    public abstract class ServicesCommandValidatorBase<TCommand> : AbstractValidator<TCommand>
        where TCommand : ServicesCommandBase
    {
        protected ServicesCommandValidatorBase(string bothSpecifiedErrorMessage, string noneSpecifiedErrorMessage)
        {
            RuleFor(command => command)
                .Must(command => !(command.DoForAllServices && command.Services.Any()))
                .WithMessage(bothSpecifiedErrorMessage);
            RuleFor(command => command)
                .Must(command => command.DoForAllServices || command.Services.Any())
                .WithMessage(noneSpecifiedErrorMessage);
        }
    }
}
