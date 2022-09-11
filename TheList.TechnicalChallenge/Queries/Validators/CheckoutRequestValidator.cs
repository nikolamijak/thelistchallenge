using FluentValidation;
using TheList.TechnicalChallenge.Queries.Requests;

namespace TheList.TechnicalChallenge.Queries.Validators
{
    public class CheckoutRequestValidator
          : AbstractValidator<CheckoutRequest>
    {
        public CheckoutRequestValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Continue;

            When(x => x != null, () =>
            {
                RuleFor(x => x.Id)
                   .Must(id => id.HasValue && id >= 0)
                   .WithMessage($"Id must be number greater than 0");
            });
        }
    }
}
