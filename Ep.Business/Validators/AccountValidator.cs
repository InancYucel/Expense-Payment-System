using FluentValidation;
using Schema;

namespace Business.Validators;

public class AccountValidator : AbstractValidator<AccountRequest>
{
    public AccountValidator()
    {
        RuleFor(x => x.StaffId)
            .NotEmpty().WithMessage("Staff ID cannot be empty");
        RuleFor(x => x.IBAN)
            .NotEmpty().WithMessage("IBAN cannot be empty")
            .Length(26).WithMessage("IBAN length must be 26 characters");
        RuleFor(x => x.Bank)
            .NotEmpty().WithMessage("Bank name cannot be empty")
            .MaximumLength(40).WithMessage("Bank name length can be a maximum of 40 characters.");
        RuleFor(x => x.CurrencyType)
            .NotEmpty().WithMessage("Currency Type cannot be empty")
            .MaximumLength(3).WithMessage("Currency Type length can be a maximum of 3 characters.");
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Currency Type cannot be empty")
            .MaximumLength(40).WithMessage("Currency Type length can be a maximum of 40 characters.");
    }
}