using System.Reflection;
using Bussiness.Queries;
using Data.DbContext;
using Data.Insert;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Expense_Payment_System;

public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly InsertRows _insertRows;
    
    public Startup(IConfiguration configuration, InsertRows instertRows)
    {
        _configuration = configuration;
        _insertRows = instertRows;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        //For EntityFramework DB
        var connection = _configuration.GetConnectionString("MsSqlConnection");
        var option = new DbContextOptionsBuilder<EpDbContext>()
            .UseSqlServer(new SqlConnection(connection))
            .Options;
        
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(StaffQueryHandler).GetTypeInfo().Assembly));
        
        services.AddDbContext<EpDbContext>(options => options.UseSqlServer(connection));
        
        services.AddControllers(); //Added Controllers folder classes
        services.AddEndpointsApiExplorer(); //  Discovers endpoints
        services.AddSwaggerGen(); //Prepares documentation for Swagger
        
        _insertRows.InitializeDatabase();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Configure the HTTP request pipeline.
        if (env.IsDevelopment()) //If we are working in a development environment, UI is enabled.
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(x => { x.MapControllers(); }); 
    }
}