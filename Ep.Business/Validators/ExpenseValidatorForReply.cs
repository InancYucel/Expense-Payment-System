using FluentValidation;
using Schema;

namespace Business.Validators;

public class ExpenseValidatorForReply : AbstractValidator<ReplyExpensesRequest>
{
    public ExpenseValidatorForReply()
    {
        RuleFor(x => x.InvoiceAmount)
            .NotEmpty().WithMessage("Invoice Amount cannot be empty");
        RuleFor(x => x.InvoiceCurrencyType)
            .NotEmpty().WithMessage("Invoice Currency Type cannot be empty")
            .MaximumLength(3).WithMessage("Invoice Currency Type Length can be a maximum of 3 characters");
    }
}