using Base.Schema;

namespace Schema;

public class ExpensePaymentOrderRequest: BaseRequest
{
    public int ExpenseId { get; set; }
    public int ExpensePaymentOrderId { get; set; }
    public DateTime PaymentConfirmationDate { get; set; }
    public string AccountConfirmingOrder { get; set; } 
    public string PaymentIban { get; set; }
    public bool IsPaymentCompleted { get; set; }
    public DateTime PaymentCompletedDate { get; set; }
    public int EftId { get; set; }
    public int SwiftId { get; set; }    
}

public class ExpensePaymentOrderResponse: BaseResponse
{
    public int Id { get; set; }
    public int ExpenseId { get; set; }
    public int ExpensePaymentOrderId { get; set; }
    public DateTime PaymentConfirmationDate { get; set; }
    public string AccountConfirmingOrder { get; set; } 
    public string PaymentIban { get; set; }
    public bool IsPaymentCompleted { get; set; }
    public DateTime PaymentCompletedDate { get; set; }
    public int EftId { get; set; }
    public int SwiftId { get; set; }    
    public bool IsActive { get; set; }
}