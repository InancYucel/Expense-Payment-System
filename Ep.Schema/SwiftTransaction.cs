using Base.Schema;

namespace Schema;

public class SwiftTransactionRequest : BaseRequest
{
    public int AccountId { get; set; }
    public string ReferenceNumber { get; set; }
    public DateTime TransactionDate { get; set; }
    
    public decimal Amount { get; set; }
    public string CurrencyType { get; set; }
    public string Description { get; set; }
    public string SenderAccount { get; set; }
    public string SenderBank { get; set; }
    public string SenderIban { get; set; }
    public string SenderName { get; set; }
    public string ReceiverBank { get; set; }
    public string ReceiverIban { get; set; }
    public string ReceiverName { get; set; }
}

public class SwiftTransactionResponse : BaseResponse
{
    public int AccountId { get; set; }
    public string ReferenceNumber { get; set; }
    public DateTime TransactionDate { get; set; }
    public decimal Amount { get; set; }
    public string CurrencyType { get; set; }
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