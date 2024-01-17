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
        if (!(_dbContext.ApplicationUsers.Any()))
        {
            InsertApplicationUserRows();
        }

        if (!(_dbContext.Staff.Any()))
        {
            InsertStaffRows();
        }

        if (!(_dbContext.Expenses.Any()))
        {
            InsertExpensesRows();
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
}

public interface IInsertRows
{
    void InitializeDatabase();
}