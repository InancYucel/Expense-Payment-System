using Data.DbContext;
using Data.Entity;
using Newtonsoft.Json;

namespace Data.Insert;

public class InsertRows : IInsertRows
{
    private readonly EpDbContext _dbContext;

    public InsertRows(EpDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void InitializeDatabase()
    {
        try
        {
            if (!(_dbContext.ApplicationUsers.Any())) 
            {
                InsertApplicationUserRows();
            }

            if (!(_dbContext.Staff.Any()))
            {
                InsertStaffRows();
            }
            if (!(_dbContext.Accounts.Any()))
            {
                InsertAccountRows();
            }
        
            if (!(_dbContext.Expenses.Any()))
            {
                InsertExpensesRows();
            }
            if (!(_dbContext.ExpensePaymentOrders.Any()))
            {
                InsertExpensePaymentOrderRows();
            }
            if (!(_dbContext.FastTransactions.Any()))
            {
                InsertFastTransactionRows();
            }
            if (!(_dbContext.SwiftTransactions.Any()))
            {
                InsertSwiftTransactionRows();
            }
            if (!(_dbContext.PaymentCategories.Any()))
            {
                InsertPaymentCategoriesRows();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return;
        }
       
    }

    private void InsertStaffRows()
    {
        try
        {
            var staffJson = new StreamReader(@"..\Ep.Data\DataToAdd\Staff.json");
            using (staffJson)
            {
                var json = staffJson.ReadToEnd();
                var root = JsonConvert.DeserializeObject<StaffRoot>(json);
                if (root != null)
                    foreach (var staff in root.Staff)
                    {
                        _dbContext.Staff.Add(staff);
                    }

                _dbContext.SaveChanges();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return;
        }
    }

    private void InsertExpensesRows()
    {
        try
        {
            var expensesJson = new StreamReader(@"..\Ep.Data\DataToAdd\Expenses.json");
            using (expensesJson)
            {
                var json = expensesJson.ReadToEnd();
                var root = JsonConvert.DeserializeObject<ExpensesRoot>(json);
                if (root != null)
                    foreach (var expense in root.Expenses)
                    {
                        _dbContext.Expenses.Add(expense);
                    }

                _dbContext.SaveChanges();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return;
        }
    }

    private void InsertApplicationUserRows()
    {
        try
        {
            var applicationUserJson = new StreamReader(@"..\Ep.Data\DataToAdd\ApplicationUser.json");
            using (applicationUserJson)
            {
                var json = applicationUserJson.ReadToEnd();
                var root = JsonConvert.DeserializeObject<ApplicationUserRoot>(json);
                if (root != null)
                    foreach (var applicationUser in root.ApplicationUser)
                    {
                        _dbContext.ApplicationUsers.Add(applicationUser);
                    }

                _dbContext.SaveChanges();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return;
        }
    }
    
    private void InsertAccountRows()
    {
        try
        {
            var accountJson = new StreamReader(@"..\Ep.Data\DataToAdd\Account.json");
            using (accountJson)
            {
                var json = accountJson.ReadToEnd();
                var root = JsonConvert.DeserializeObject<AccountRoot>(json);
                if (root != null)
                    foreach (var account in root.Account)
                    {
                        _dbContext.Accounts.Add(account);
                    }
                _dbContext.SaveChanges();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return;
        }
    }
    
    private void InsertExpensePaymentOrderRows()
    {
        try
        {
            var expensePaymentOrderJson = new StreamReader(@"..\Ep.Data\DataToAdd\ExpensePaymentOrder.json");
            using (expensePaymentOrderJson)
            {
                var json = expensePaymentOrderJson.ReadToEnd();
                var root = JsonConvert.DeserializeObject<ExpensePaymentOrderRoot>(json);
                if (root != null)
                    foreach (var expensePaymentOrder in root.ExpensePaymentOrder)
                    {
                        _dbContext.ExpensePaymentOrders.Add(expensePaymentOrder);
                    }
                _dbContext.SaveChanges();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return;
        }
    }
    
    private void InsertFastTransactionRows()
    {
        try
        {
            var fastTransactionJson = new StreamReader(@"..\Ep.Data\DataToAdd\FastTransaction.json");
            using (fastTransactionJson)
            {
                var json = fastTransactionJson.ReadToEnd();
                var root = JsonConvert.DeserializeObject<FastTransactionRoot>(json);
                if (root != null)
                    foreach (var fastTransaction in root.FastTransaction)
                    {
                        _dbContext.FastTransactions.Add(fastTransaction);
                    }
                _dbContext.SaveChanges();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return;
        }
    }
    
    private void InsertSwiftTransactionRows()
    {
        try
        {
            var swiftTransactionJson = new StreamReader(@"..\Ep.Data\DataToAdd\SwiftTransaction.json");
            using (swiftTransactionJson)
            {
                var json = swiftTransactionJson.ReadToEnd();
                var root = JsonConvert.DeserializeObject<SwiftTransactionRoot>(json);
                if (root != null)
                    foreach (var swiftTransaction in root.SwiftTransaction)
                    {
                        _dbContext.SwiftTransactions.Add(swiftTransaction);
                    }
                _dbContext.SaveChanges();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return;
        }
    }
    
    private void InsertPaymentCategoriesRows()
    {
        try
        {
            var paymentCategoriesJson = new StreamReader(@"..\Ep.Data\DataToAdd\PaymentCategories.json");
            using (paymentCategoriesJson)
            {
                var json = paymentCategoriesJson.ReadToEnd();
                var root = JsonConvert.DeserializeObject<PaymentCategoriesRoot>(json);
                if (root != null)
                    foreach (var paymentCategories in root.PaymentCategories)
                    {
                        _dbContext.PaymentCategories.Add(paymentCategories);
                    }
                _dbContext.SaveChanges();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return;
        }
    }

    private class StaffRoot
    {
        public List<Staff> Staff { get; set; }
    }
    private class ExpensesRoot
    {
        public List<Expenses> Expenses { get; set; }
    }
    private class ApplicationUserRoot
    {
        public List<ApplicationUser> ApplicationUser { get; set; }
    }
    private class AccountRoot
    {
        public List<Account> Account { get; set; }
    }
    private class ExpensePaymentOrderRoot
    {
        public List<ExpensePaymentOrder> ExpensePaymentOrder { get; set; }
    }
    private class FastTransactionRoot
    {
        public List<FastTransaction> FastTransaction { get; set; }
    }
    private class SwiftTransactionRoot
    {
        public List<SwiftTransaction> SwiftTransaction { get; set; }
    }
    private class PaymentCategoriesRoot
    {
        public List<PaymentCategories> PaymentCategories { get; set; }
    }
}

public interface IInsertRows
{
    void InitializeDatabase();
}