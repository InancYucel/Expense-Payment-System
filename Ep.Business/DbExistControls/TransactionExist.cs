using Data.DbContext;
using Data.Entity;

namespace Business.DbExistControls;

public class TransactionExist
{
    private readonly EpDbContext _dbContext;

    public TransactionExist(EpDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool IsReferenceNumberExistInFastTransaction(string referenceNumber)
    {
        var fromDb = _dbContext.Set<FastTransaction>().FirstOrDefault(x => x.ReferenceNumber == referenceNumber);
        return fromDb != null;
    }
    public bool IsReferenceNumberExistInSwiftTransaction(string referenceNumber)
    {
        var fromDb = _dbContext.Set<SwiftTransaction>().FirstOrDefault(x => x.ReferenceNumber == referenceNumber);
        return fromDb != null;
    }
}