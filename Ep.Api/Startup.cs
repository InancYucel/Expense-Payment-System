using System.Reflection;
using System.Text;
using AutoMapper;
using Base.Token;
using Business.Mapper;
using Business.Queries;
using Data.DbContext;
using Data.Insert;
using Expense_Payment_System.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

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

        //JwtConfig Begin
        var jwtConfig = _configuration.GetSection("JwtConfig").Get<JwtConfig>();
        services.Configure<JwtConfig>(_configuration.GetSection("JwtConfig"));
        
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = true;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtConfig.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig.Secret)),
                ValidAudience = jwtConfig.Audience,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(2)
            };
        });
        //JwtConfig End
        
        //Swagger Authorize button enable Begin
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Vb Api Management", Version = "v1.0" });

            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Vb Management for IT Company",
                Description = "Enter JWT Bearer token **_only_**",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };
            c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { securityScheme, new string[] { } }
            });
        });
        ////Swagger Authorize button enable End
        
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

        app.UseMiddleware<WitcherMiddleware>();
        app.UseMiddleware<ErrorHandlerMiddleware>();

        app.UseHttpsRedirection();
        app.UseAuthentication(); //JWT
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(x => { x.MapControllers(); }); 
        ins.InitializeDatabase();
    }
}