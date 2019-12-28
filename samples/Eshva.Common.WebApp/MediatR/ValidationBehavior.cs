#region Usings

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

#endregion


namespace Eshva.Common.WebApp.MediatR
{
    public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next
        )
        {
            var context = new ValidationContext(request);
            var failures = _validators
                           .Select(validator => validator.Validate(context))
                           .SelectMany(result => result.Errors)
                           .Where(failure => failure != null)
                           .ToList();

            if (failures.Any())
            {
                throw new ValidationException(failures);
            }

            return next();
        }

        private readonly IEnumerable<IValidator<TRequest>> _validators;
    }
}
