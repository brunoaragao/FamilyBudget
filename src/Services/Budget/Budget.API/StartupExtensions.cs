using System.Reflection;

using Budget.Domain.AggregateModels.ExpenseAggregates;
using Budget.Domain.AggregateModels.IncomeAggregates;
using Budget.Domain.SeedWork;
using Budget.Domain.Services;
using Budget.Infrastructure.Data;
using Budget.Infrastructure.Data.Repositories;
using Budget.Infrastructure.Data.Seed;

using FluentValidation;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Budget.API;

public static class StartupExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<BudgetContext>(options =>
        {
            options.UseSqlServer("Name=BudgetConnection",
                sqlOptions => sqlOptions.MigrationsAssembly("Budget.Infrastructure"));

            options.EnableSensitiveDataLogging();
        });

        builder.Services.AddScoped<ISeeder, DemoDataSeeder>();

        builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
        builder.Services.AddScoped<IIncomeRepository, IncomeRepository>();

        builder.Services.AddScoped<IBudgetSummaryService, BudgetSummaryService>();

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        builder.Services.AddMediatR(config =>
            config.RegisterServicesFromAssembly(Assembly.Load("Budget.Application")));

        builder.Services.AddValidatorsFromAssembly(Assembly.Load("Budget.Application"));

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = builder.Configuration["IdentityServer:Authority"];
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false
                };
            });


        builder.Services.AddAuthorization(options => options.AddPolicy("ApiScope", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "budget");
            }));

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new() { Title = "Budget.API", Version = "v1" });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Enter the Bearer Authorization string as following: `Bearer {token-without-brackets}`",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new List<string>()
                }
            });
        });

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Budget.API v1"));
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers().RequireAuthorization("ApiScope");

        return app;
    }

    public static async Task SeedData(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<ISeeder>();
        await seeder.SeedAsync();
    }
}