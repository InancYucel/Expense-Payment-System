using Data.DbContext;
using Data.Entity;

namespace Business.DbExistControls;

public class ExpensePaymentOrderExist
{
    private readonly EpDbContext _dbContext;

    public ExpensePaymentOrderExist(EpDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public bool IsExpenseIdIsExist(int expenseId) //Is there another one with the same Expense ID?
    {
        var fromDb = _dbContext.Set<ExpensePaymentOrder>().FirstOrDefault(x => x.ExpenseId == expenseId);
        return fromDb != null;
    }
    
    public bool IsExpensePaymentOrderIdIsExist(int expensePaymentOrderId) //Is there another one with the same ExpensePaymentOrderId?
    {
        var fromDb = _dbContext.Set<ExpensePaymentOrder>().FirstOrDefault(x => x.Id == expensePaymentOrderId);
        return fromDb != null;
    }
}