using Base.Schema;

namespace Schema;

public class ExpenseRequest : BaseRequest
{
    public int StaffId { get; set; }
    public int ExpenseNumber { get; set; }
    public double InvoiceReferenceNumber { get; set; }
    public double InvoiceAmount { get; set; }
    public DateTime InvoiceDate { get; set; }
    public string InvoiceDescription { get; set; }
    public string ExpenseClaimDescription { get; set; }
    public string ExpenseRequestStatus { get; set; } 
}

public class ExpenseResponse : BaseResponse
{
    public int StaffId { get; set; }
    public int ExpenseNumber { get; set; }
    public double InvoiceReferenceNumber { get; set; }
    public double InvoiceAmount { get; set; }
    public string InvoiceDescription { get; set; }
    public string ExpenseClaimDescription { get; set; }
    public string ExpenseRequestStatus { get; set; }
}