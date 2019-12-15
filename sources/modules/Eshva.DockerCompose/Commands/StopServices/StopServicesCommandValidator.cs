#region Usings

using System.Linq;
using FluentValidation;

#endregion


namespace Eshva.DockerCompose.Commands.StopServices
{
    /// <summary>
    /// Validator for the <see cref="StopServicesCommand"/>.
    /// </summary>
    public sealed class StopServicesCommandValidator : AbstractValidator<StopServicesCommand>
    {
        /// <inheritdoc cref="AbstractValidator{T}"/>
        public StopServicesCommandValidator()
        {
            RuleFor(command => command)
                .Must(command => !(command.DoStopAllServices && command.Services.Any()))
                .WithMessage(
                    "It's not allowed to configure all services and specify to stop some of them. " +
                    "StopAllServices and StopServices shouldn't be used together.");
            RuleFor(command => command)
                .Must(command => command.DoStopAllServices || command.Services.Any())
                .WithMessage(
                    "You should specify to stop all services or some of them. " +
                    "Use StopAllServices or StopServices method.");
        }
    }
}
