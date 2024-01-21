namespace Schema;

public class StaffRequest 
{
    public string IdentityNumber { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime LastActivityDate { get; set; }
}

public class StaffResponse 
{
    public int Id { get; set; }
    public string IdentityNumber { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime LastActivityDate { get; set; }
    public bool IsActive { get; set; }
}