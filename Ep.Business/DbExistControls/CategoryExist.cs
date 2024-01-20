using Data.DbContext;
using Data.Entity;

namespace Business.DbExistControls;

public class CategoryExist
{
    private readonly EpDbContext _dbContext;

    public CategoryExist(EpDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool IsCategoryExist(string category)
    {
        var fromDb = _dbContext.Set<PaymentCategories>().FirstOrDefault(x => x.Category == category);
        return fromDb != null;
    }
}