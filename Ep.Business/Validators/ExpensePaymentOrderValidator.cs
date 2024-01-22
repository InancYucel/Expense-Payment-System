using FluentValidation;
using Schema;

namespace Business.Validators;

public class ExpensePaymentOrderValidator : AbstractValidator<ExpensePaymentOrderRequest>
{
    public ExpensePaymentOrderValidator()
    {
        RuleFor(x => x.ExpenseId)
            .NotEmpty().WithMessage("Expense ID cannot be empty");
        RuleFor(x => x.PaymentConfirmationDate)
            .NotEmpty().WithMessage("Payment Confirmation Date cannot be empty");
        RuleFor(x => x.AccountConfirmingOrder)
            .NotEmpty().WithMessage("Account Confirming Order cannot be empty")
            .MaximumLength(20).WithMessage("Account Confirming Order Length can be a maximum of 20 characters");
        RuleFor(x => x.AccountConfirmingOrder)
            .NotEmpty().WithMessage("Account Confirming Order cannot be empty")
            .MaximumLength(20).WithMessage("Account Confirming Order Length can be a maximum of 20 characters");
        RuleFor(x => x.PaymentIban)
            .NotEmpty().WithMessage("IBAN cannot be empty")
            .Length(26).WithMessage("IBAN length must be 26 characters");
        RuleFor(x => x.PaymentCategory)
            .NotEmpty().WithMessage("Payment Category cannot be empty")
            .MaximumLength(24).WithMessage("Payment Category length must be 24 characters");
    }
}