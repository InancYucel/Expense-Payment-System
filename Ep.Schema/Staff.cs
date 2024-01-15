using Base.Schema;

namespace Schema;

public class StaffRequest : BaseRequest
{
    public int StaffNumber { get; set; }
    public string IdentityNumber { get; set; }
    public string IBAN { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public class StaffResponse : BaseResponse
{
    public int StaffNumber { get; set; }
    public string IdentityNumber { get; set; }
    public string IBAN { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}