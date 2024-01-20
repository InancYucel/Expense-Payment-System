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

    public bool IsReferenceNumberExistInFastTransaction(string referenceNumber) //Is there another one with the same Reference Number in Fast Transaction
    {
        var fromDb = _dbContext.Set<FastTransaction>().FirstOrDefault(x => x.ReferenceNumber == referenceNumber);
        return fromDb != null;
    }
    public bool IsReferenceNumberExistInSwiftTransaction(string referenceNumber) //Is there another one with the same Reference Number in Swift Transaction
    {
        var fromDb = _dbContext.Set<SwiftTransaction>().FirstOrDefault(x => x.ReferenceNumber == referenceNumber);
        return fromDb != null;
    }
        
    public bool IsReferenceExistInFastTransaction(string referenceNumber) //Is there another one with the same Reference?
    {
        var fromDb = _dbContext.Set<FastTransaction>().FirstOrDefault(x => x.ReferenceNumber == referenceNumber);
        return fromDb != null;
    }
    
    public bool IsReferenceExistInSwiftTransaction(string referenceNumber) //Is there another one with the same Reference?
    {
        var fromDb = _dbContext.Set<SwiftTransaction>().FirstOrDefault(x => x.ReferenceNumber == referenceNumber);
        return fromDb != null;
    }
    
}