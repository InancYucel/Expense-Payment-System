namespace Schema;

public class FastTransactionRequestForUpdate 
{
    public int ExpensePaymentOrderId { get; set; }
    public string ReferenceNumber { get; set; }
    public DateTime TransactionDate { get; set; }
    public double Amount { get; set; }
    public string Description { get; set; }
    public string SenderBank { get; set; }
    public string SenderIban { get; set; }
    public string SenderName { get; set; }
    public string ReceiverBank { get; set; }
    public string ReceiverIban { get; set; }
    public string ReceiverName { get; set; }
}

public class FastTransactionRequest : FastTransactionRequestForUpdate
{
    public int AccountId { get; set; }
}

public class FastTransactionResponse 
{
    public int AccountId { get; set; }
    public int ExpensePaymentOrderId { get; set; }
    public string ReferenceNumber { get; set; }
    public DateTime TransactionDate { get; set; }
    public double Amount { get; set; }
    public string Description { get; set; }
    public string SenderBank { get; set; }
    public string SenderIban { get; set; }
    public string SenderName { get; set; }
    public string ReceiverBank { get; set; }
    public string ReceiverIban { get; set; }
    public string ReceiverName { get; set; }
    public bool IsActive { get; set; }
}