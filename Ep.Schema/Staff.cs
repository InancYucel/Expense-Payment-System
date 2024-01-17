using Base.Schema;

namespace Schema;

public class StaffRequest : BaseRequest
{
    public int Id { get; set; }
    public string IdentityNumber { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime LastActivityDate { get; set; }
}

public class StaffResponse : BaseResponse
{
    public int Id { get; set; }
    public string IdentityNumber { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime LastActivityDate { get; set; }
    public bool IsActive { get; set; }
}