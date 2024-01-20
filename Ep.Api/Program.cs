using Data.Insert;

namespace Expense_Payment_System;

public class Program
{
    public static void Main(string[] args)
    {
        Host.CreateDefaultBuilder(args)
            .ConfigureServices(serviceCollection =>
                serviceCollection.AddScoped<IInsertRows, InsertRows>()) //To use dependency injection in Startup.cs
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            }).Build().Run();
    }
}