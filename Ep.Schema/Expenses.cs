using System.Text.Json.Serialization;

namespace Schema;

public class ExpensesRequest
{
    public int StaffId { get; set; }
    public string InvoiceReferenceNumber { get; set; }
    public double InvoiceAmount { get; set; }
    public string InvoiceCurrencyType { get; set; }
    public string InvoiceDate { get; set; }
    public string InvoiceCategory { get; set; }
    public string PaymentInstrument { get; set; }
    public string PaymentLocation { get; set; }
    public string ExpenseClaimDescription { get; set; }
}

public class ExpensesResponse
{
    public int Id { get; set; }
    public int StaffId { get; set; }
    public string InvoiceReferenceNumber { get; set; }
    public double InvoiceAmount { get; set; }
    public string InvoiceDate { get; set; }
    public string InvoiceCurrencyType { get; set; }
    public string InvoiceCategory { get; set; }
    public string PaymentInstrument { get; set; }
    public string PaymentLocation { get; set; }
    public string ExpenseClaimDescription { get; set; }
    public string ExpenseRequestStatus { get; set; }
    public string ExpensePaymentRefusal { get; set; }
    public bool IsActive { get; set; }
}

public class StaffExpensesRequest
{
    public string InvoiceReferenceNumber { get; set; }
    public double InvoiceAmount { get; set; }
    public string InvoiceCurrencyType { get; set; }
    public string InvoiceDate { get; set; }
    public string InvoiceCategory { get; set; }
    public string PaymentInstrument { get; set; }
    public string PaymentLocation { get; set; }
    public string ExpenseClaimDescription { get; set; }
    public string ExpenseRequestStatus { get; set; } 
    public string ExpensePaymentRefusal { get; set; }
}

public class ReplyExpensesRequest
{
    [JsonIgnore]
    public int StaffId { get; set; }
    public double InvoiceAmount { get; set; }
    public string InvoiceCurrencyType { get; set; }
    public string ExpenseCategory { get; set; }
    public string ExpenseRequestStatus { get; set; } 
    public string ExpensePaymentRefusal { get; set; }
}

public class ExpensesRequestForUpdate 
{
    public string InvoiceReferenceNumber { get; set; }
    public double InvoiceAmount { get; set; }
    public string InvoiceCurrencyType { get; set; }
    public string InvoiceCategory { get; set; }
    public string InvoiceDate { get; set; }
    public string PaymentInstrument { get; set; }
    public string PaymentLocation { get; set; }
    public string ExpenseClaimDescription { get; set; }
}