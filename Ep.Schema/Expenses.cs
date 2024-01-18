using Base.Schema;

namespace Schema;

public class ExpensesRequest : BaseRequest
{
    public int StaffId { get; set; }
    public string InvoiceReferenceNumber { get; set; }
    public double InvoiceAmount { get; set; }
    public DateTime InvoiceDate { get; set; }
    public string InvoiceDescription { get; set; }
    public string ExpenseClaimDescription { get; set; }
    public string ExpenseRequestStatus { get; set; } 
}

public class ExpensesResponse : BaseResponse
{
    public int Id { get; set; }
    public int StaffId { get; set; }
    public string InvoiceReferenceNumber { get; set; }
    public double InvoiceAmount { get; set; }
    public string InvoiceDescription { get; set; }
    public string ExpenseClaimDescription { get; set; }
    public string ExpenseRequestStatus { get; set; }
    public string ExpensePaymentRefusal { get; set; }
    public bool IsActive { get; set; }
}

public class StaffExpensesRequest : BaseRequest
{
    public string InvoiceReferenceNumber { get; set; }
    public double InvoiceAmount { get; set; }
    public DateTime InvoiceDate { get; set; }
    public string InvoiceDescription { get; set; }
    public string ExpenseClaimDescription { get; set; }
    public string ExpenseRequestStatus { get; set; } 
    public string ExpensePaymentRefusal { get; set; }
}

public class ReplyExpensesRequest : BaseRequest
{
    public string ExpenseRequestStatus { get; set; } 
    public string ExpensePaymentRefusal { get; set; }
}