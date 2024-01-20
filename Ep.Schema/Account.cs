using Base.Schema;

namespace Schema;


public class AccountRequestForUpdate : BaseRequest
{
    public string IBAN { get; set; }
    public string Bank { get; set; }
    public string CurrencyType { get; set; }
    public string Name { get; set; }
}

public class AccountRequest : AccountRequestForUpdate
{
    public int StaffId { get; set; }
}

public class AccountResponse : BaseResponse
{
    public int StaffId { get; set; }
    public string IBAN { get; set; }
    public string Bank { get; set; }
    public decimal Balance { get; set; }
    public string CurrencyType { get; set; }
    public string Name { get; set; }
    public DateTime OpenDate { get; set; }
    public bool IsActive { get; set; }
}
