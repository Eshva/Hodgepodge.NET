#region Usings

using System.Linq;
using FluentValidation;

#endregion


namespace Eshva.DockerCompose.Commands.StartServices
{
    /// <summary>
    /// Validator for the <see cref="StartServicesCommand"/>.
    /// </summary>
    public sealed class StartServicesCommandValidator : AbstractValidator<StartServicesCommand>
    {
        /// <inheritdoc cref="AbstractValidator{T}"/>
        public StartServicesCommandValidator()
        {
            RuleFor(command => command)
                .Must(command => !(command.DoStartAllServices && command.Services.Any()))
                .WithMessage(
                    "It's not allowed to configure all services and specify to start some of them. " +
                    "StartAllServices and StartServices shouldn't be used together.");
            RuleFor(command => command)
                .Must(command => command.DoStartAllServices || command.Services.Any())
                .WithMessage(
                    "You should specify to start all services or some of them. " +
                    "Use StartAllServices or StartServices method.");
        }
    }
}
