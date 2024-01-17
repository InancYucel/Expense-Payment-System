using Base.Schema;

namespace Schema;

public class AccountRequest : BaseRequest
{
    public int AccountId { get; set; }
    public int StaffId { get; set; }
    public string IBAN { get; set; }
    public decimal Balance { get; set; }
    public string CurrencyType { get; set; }
    public string Name { get; set; }
}

public class AccountResponse : BaseResponse
{
    public int AccountId { get; set; }
    public int StaffId { get; set; }
    public int AccountNumber { get; set; }
    public string IBAN { get; set; }
    public decimal Balance { get; set; }
    public string CurrencyType { get; set; }
    public string Name { get; set; }
    public DateTime OpenDate { get; set; }
}