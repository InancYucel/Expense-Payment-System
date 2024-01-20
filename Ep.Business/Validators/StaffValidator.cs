using FluentValidation;
using Schema;

namespace Business.Validators;

public class StaffValidator : AbstractValidator<StaffRequest>
{
    public StaffValidator()
    {
        RuleFor(x => x.IdentityNumber)
            .NotEmpty().WithMessage("Identity Number cannot be empty")
            .Length(11).WithMessage("IBAN length must be 26 characters");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First Name cannot be empty")
            .Length(50).WithMessage("First Name length can be a maximum of 50 characters");
        
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last Name cannot be empty")
            .Length(50).WithMessage("Last Name length can be a maximum of 50 characters");

        RuleFor(x => x.LastActivityDate)
            .NotEmpty().WithMessage("Last Activity Date cannot be empty");
    }
}