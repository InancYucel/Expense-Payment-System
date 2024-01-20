using FluentValidation;
using Schema;

namespace Business.Validators;

public class ExpenseValidator : AbstractValidator<ExpensesRequest>
{
    public ExpenseValidator()
    {
        RuleFor(x => x.StaffId)
            .NotEmpty().WithMessage("Staff ID cannot be empty");
        RuleFor(x => x.InvoiceReferenceNumber)
            .NotEmpty().WithMessage("Invoice Reference Number cannot be empty");
        RuleFor(x => x.InvoiceAmount)
            .NotEmpty().WithMessage("Invoice Amount cannot be empty");
        RuleFor(x => x.InvoiceCurrencyType)
            .NotEmpty().WithMessage("Invoice Currency Type cannot be empty")
            .MaximumLength(3).WithMessage("Invoice Currency Type Length can be a maximum of 3 characters");
        RuleFor(x => x.InvoiceDate)
            .NotEmpty().WithMessage("Invoice Date cannot be empty");
        RuleFor(x => x.InvoiceCategory)
            .NotEmpty().WithMessage("Invoice Category cannot be empty")
            .MaximumLength(15).WithMessage("Invoice Currency Type Length can be a maximum of 3 characters");
        RuleFor(x => x.PaymentInstrument)
            .NotEmpty().WithMessage("Payment Instrument cannot be empty")
            .MaximumLength(15).WithMessage("Payment Instrument Length can be a maximum of 15 characters");
        RuleFor(x => x.PaymentLocation)
            .NotEmpty().WithMessage("Payment Location cannot be empty")
            .MaximumLength(40).WithMessage("Payment Location Length can be a maximum of 40 characters");
        RuleFor(x => x.ExpenseClaimDescription)
            .NotEmpty().WithMessage("Expense Claim Description cannot be empty")
            .MaximumLength(40).WithMessage("Expense Claim Description Length can be a maximum of 40 characters");
    }
}