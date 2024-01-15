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

    public void InsertStaffRows()
    {
        Root? root;
        try
        {
            var staffJson = new StreamReader(@"..\Ep.Data\DataToAdd\Staff.json");
            using (staffJson)
            {
                var json = staffJson.ReadToEnd();
                root = JsonConvert.DeserializeObject<Root>(json);
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
    
    public void InitializeDatabase()
    {
        if (!(_dbContext.Staff.Any()))
        {
            InsertStaffRows();
        }
    }
    
    private class Root
    {
        public List<Staff> Staff { get; set; }
    }
}

public interface IInsertRows
{
    void InitializeDatabase();
}