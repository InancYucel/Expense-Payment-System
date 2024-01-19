using Base.Schema;

namespace Schema;

public class FastTransactionRequest : BaseRequest
{
    public int AccountId { get; set; }
    public int ExpensePaymentOrderId { get; set; }
    public string ReferenceNumber { get; set; }
    public DateTime TransactionDate { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public string SenderAccount { get; set; }
    public string SenderBank { get; set; }
    public string SenderIban { get; set; }
    public string SenderName { get; set; }
    public string ReceiverBank { get; set; }
    public string ReceiverIban { get; set; }
    public string ReceiverName { get; set; }
}

public class FastTransactionResponse : BaseResponse
{
    public int AccountId { get; set; }
    public int ExpensePaymentOrderId { get; set; }
    public string ReferenceNumber { get; set; }
    public DateTime TransactionDate { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public string SenderAccount { get; set; }
    public string SenderBank { get; set; }
    public string SenderIban { get; set; }
    public string SenderName { get; set; }
    public string ReceiverBank { get; set; }
    public string ReceiverIban { get; set; }
    public string ReceiverName { get; set; }
    public bool IsActive { get; set; }
}