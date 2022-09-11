using FluentValidation;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;

namespace TheList.TechnicalChallenge.Exceptions
{
    public class CustomValidationException : ValidationException
    {
        private const string DefaultMessage = "One or more validation failures have occurred.";

        public string Code { get; } = "validation_error";

        public IDictionary<string, string[]> Failures { get; private set; }
        public CustomValidationException() : this(DefaultMessage)
        {
        }

        public CustomValidationException(string message) : base(message)
        {
        }

        public CustomValidationException(IDictionary<string, string[]> failures)
            : this(DefaultMessage,failures)
        {
        }

        public CustomValidationException(IEnumerable<ValidationFailure> failures)
            : this(DefaultMessage, failures)
        {
        }

        public CustomValidationException(string message, IEnumerable<ValidationFailure> failures)
            : base(message, failures)
        {
            Failures = new Dictionary<string, string[]>();
            var propertyNames = failures
                .Select(e => e.PropertyName)
                .ToArray()
                .Distinct();

            foreach (var propertyName in propertyNames)
            {
                var propertyFailures = failures
                    .Where(e => e.PropertyName == propertyName)
                    .Select(e => e.ErrorMessage)
                    .Distinct()
                    .ToArray();

                if (!Failures.ContainsKey(propertyName))
                {
                    Failures.Add(propertyName, propertyFailures);
                }
            }
        }

        public CustomValidationException(string message, IDictionary<string,string[]> failures)
          : base(message)
        {
            Failures = failures;
        }
    }
}
