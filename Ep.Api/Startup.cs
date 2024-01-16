using System.Reflection;
using AutoMapper;
using Business.Mapper;
using Business.Queries;
using Data.DbContext;
using Data.Insert;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Expense_Payment_System;

public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly IInsertRows _insertRows;
    
    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        //For EntityFramework DB
        var connection = _configuration.GetConnectionString("MsSqlConnection");
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(StaffQueryHandler).GetTypeInfo().Assembly));
        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MapperConfig()));
        services.AddSingleton(mapperConfig.CreateMapper());
        services.AddDbContext<EpDbContext>(options => options.UseSqlServer(connection));
        services.AddScoped<IInsertRows, InsertRows>();
        services.AddControllers(); //Added Controllers folder classes
        
        services.AddEndpointsApiExplorer(); //  Discovers endpoints
        services.AddSwaggerGen(); //Prepares documentation for Swagger
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IInsertRows ins)
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
        ins.InitializeDatabase();
    }
}