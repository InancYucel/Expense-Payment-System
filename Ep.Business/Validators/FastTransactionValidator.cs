using FluentValidation;
using Schema;

namespace Business.Validators;

public class FastTransactionValidator : AbstractValidator<FastTransactionRequest>
{
    public FastTransactionValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("Account ID cannot be empty");
        RuleFor(x => x.ExpensePaymentOrderId)
            .NotEmpty().WithMessage("Expense Payment Order ID cannot be empty");
        RuleFor(x => x.ReferenceNumber)
            .NotEmpty().WithMessage("Reference Number cannot be empty")
            .MaximumLength(10).WithMessage("Reference Number Length can be a maximum of 10 characters");
        RuleFor(x => x.TransactionDate)
            .NotEmpty().WithMessage("Transaction Date cannot be empty");
        RuleFor(x => x.Amount)
            .NotEmpty().WithMessage("Amount cannot be empty");
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description cannot be empty")
            .MaximumLength(50).WithMessage("Description Length can be a maximum of 50 characters");
        RuleFor(x => x.SenderBank)
            .NotEmpty().WithMessage("Sender Bank cannot be empty")
            .MaximumLength(50).WithMessage("Sender Bank Length can be a maximum of 50 characters");
        RuleFor(x => x.SenderIban)
            .NotEmpty().WithMessage("Sender Iban cannot be empty")
            .MaximumLength(50).WithMessage("Sender Iban Length can be a maximum of 26 characters");
        RuleFor(x => x.SenderName)
            .NotEmpty().WithMessage("Sender Name cannot be empty")
            .MaximumLength(50).WithMessage("Sender Name Length can be a maximum of 50 characters");
        RuleFor(x => x.ReceiverBank)
            .NotEmpty().WithMessage("Receiver Bank cannot be empty")
            .MaximumLength(50).WithMessage("Receiver Bank Length can be a maximum of 50 characters");
        RuleFor(x => x.ReceiverIban)
            .NotEmpty().WithMessage("Receiver Iban cannot be empty")
            .MaximumLength(26).WithMessage("Receiver Iban Length can be a maximum of 26 characters");
        RuleFor(x => x.ReceiverName)
            .NotEmpty().WithMessage("Receiver Name cannot be empty")
            .MaximumLength(50).WithMessage("Receiver Name Length can be a maximum of 50 characters");
    }
}

public class FastTransactionValidatorForUpdateRequest : AbstractValidator<FastTransactionRequestForUpdate>
{
    public FastTransactionValidatorForUpdateRequest()
    {
        RuleFor(x => x.ExpensePaymentOrderId)
            .NotEmpty().WithMessage("Expense Payment Order ID cannot be empty");
        RuleFor(x => x.ReferenceNumber)
            .NotEmpty().WithMessage("Reference Number cannot be empty")
            .MaximumLength(10).WithMessage("Reference Number Length can be a maximum of 10 characters");
        RuleFor(x => x.TransactionDate)
            .NotEmpty().WithMessage("Transaction Date cannot be empty");
        RuleFor(x => x.Amount)
            .NotEmpty().WithMessage("Amount cannot be empty");
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description cannot be empty")
            .MaximumLength(50).WithMessage("Description Length can be a maximum of 50 characters");
        RuleFor(x => x.SenderBank)
            .NotEmpty().WithMessage("Sender Bank cannot be empty")
            .MaximumLength(50).WithMessage("Sender Bank Length can be a maximum of 50 characters");
        RuleFor(x => x.SenderIban)
            .NotEmpty().WithMessage("Sender Iban cannot be empty")
            .MaximumLength(50).WithMessage("Sender Iban Length can be a maximum of 26 characters");
        RuleFor(x => x.SenderName)
            .NotEmpty().WithMessage("Sender Name cannot be empty")
            .MaximumLength(50).WithMessage("Sender Name Length can be a maximum of 50 characters");
        RuleFor(x => x.ReceiverBank)
            .NotEmpty().WithMessage("Receiver Bank cannot be empty")
            .MaximumLength(50).WithMessage("Receiver Bank Length can be a maximum of 50 characters");
        RuleFor(x => x.ReceiverIban)
            .NotEmpty().WithMessage("Receiver Iban cannot be empty")
            .MaximumLength(26).WithMessage("Receiver Iban Length can be a maximum of 26 characters");
        RuleFor(x => x.ReceiverName)
            .NotEmpty().WithMessage("Receiver Name cannot be empty")
            .MaximumLength(50).WithMessage("Receiver Name Length can be a maximum of 50 characters");
        
    }
}