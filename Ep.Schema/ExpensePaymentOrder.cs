namespace Schema;

public class ExpensePaymentOrderRequest
{
    public int ExpenseId { get; set; }
    public DateTime PaymentConfirmationDate { get; set; }
    public string AccountConfirmingOrder { get; set; } 
    public string PaymentIban { get; set; }
    public string PaymentCategory { get; set; }
    public bool IsPaymentCompleted { get; set; }
    public DateTime PaymentCompletedDate { get; set; }
}

public class ExpensePaymentOrderResponse
{
    public int Id { get; set; }
    public int ExpenseId { get; set; }
    public DateTime PaymentConfirmationDate { get; set; }
    public string AccountConfirmingOrder { get; set; } 
    public string PaymentIban { get; set; }
    public string PaymentCategory { get; set; }
    public bool IsPaymentCompleted { get; set; }
    public DateTime PaymentCompletedDate { get; set; }
    public bool IsActive { get; set; }
}