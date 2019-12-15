#region Usings

using System.Linq;
using FluentValidation;

#endregion


namespace Eshva.DockerCompose.Commands.Logs
{
    /// <summary>
    /// Validator for the <see cref="LogsCommand"/>.
    /// </summary>
    public sealed class LogsCommandValidator : AbstractValidator<LogsCommand>
    {
        /// <inheritdoc cref="AbstractValidator{T}"/>
        public LogsCommandValidator()
        {
            RuleFor(command => command)
                .Must(command => !(command.DoTakeFromAllServices && command.FromServices.Any()))
                .WithMessage(
                    "It's not allowed to configure all services and specify to start some of them. " +
                    "FromAllServices and FromServices shouldn't be used together.");
            RuleFor(command => command)
                .Must(command => command.DoTakeFromAllServices || command.FromServices.Any())
                .WithMessage(
                    "You should specify to take logs from all services or from some of them. " +
                    "Use FromAllServices or FromServices method.");
        }
    }
}
