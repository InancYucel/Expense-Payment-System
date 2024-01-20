using FluentValidation;
using Schema;

namespace Business.Validators;

public class PaymentCategoriesValidator : AbstractValidator<PaymentCategoriesRequest>
{
    public PaymentCategoriesValidator()
    {
        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Category cannot be empty")
            .MaximumLength(20).WithMessage("");
    }
}