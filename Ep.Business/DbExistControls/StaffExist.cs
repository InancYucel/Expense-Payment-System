using Data.DbContext;
using Data.Entity;

namespace Business.DbExistControls;

public class StaffExist
{
    private readonly EpDbContext _dbContext;

    public StaffExist(EpDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public bool IsStaffExist(int staffId)
    {
        var fromDb = _dbContext.Set<Staff>().FirstOrDefault(x => x.Id == staffId);
        return fromDb != null;
    }
    public bool IsIdentityNumberExist(string identityNumber)
    {
        var fromDb = _dbContext.Set<Staff>().FirstOrDefault(x => x.IdentityNumber == identityNumber);
        return fromDb != null;
    }
    
}