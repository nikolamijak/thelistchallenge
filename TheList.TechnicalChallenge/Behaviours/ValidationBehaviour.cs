using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using FluentValidation;
using System.Linq;
using System.Diagnostics;
using TheList.TechnicalChallenge.Exceptions;

namespace TheList.TechnicalChallenge.Behaviours
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
           where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (_validators != null && _validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var failures = _validators
                    .Select(v => v.Validate(context))
                    .SelectMany(result => result.Errors)
                    .Where(f => f != null)
                    .Distinct();
                if (failures.Any())
                {                    
                    throw new CustomValidationException(failures);
                }
               
            }

            return next();
        }
    }
}
