using Data.DbContext;
using Data.Entity;

namespace Business.DbExistControls;

public class AccountExist
{
    private readonly EpDbContext _dbContext;

    public AccountExist(EpDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool IsIbanExist(string iban)
    {
        var fromDb = _dbContext.Set<Account>().FirstOrDefault(x => x.IBAN == iban);
        return fromDb != null;
    }
}