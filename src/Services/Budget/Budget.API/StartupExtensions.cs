using System.Reflection;

using Budget.Application.Behaviors;
using Budget.Domain.AggregateModels.ExpenseAggregates;
using Budget.Domain.AggregateModels.IncomeAggregates;
using Budget.Domain.SeedWork;
using Budget.Domain.Services;
using Budget.Infrastructure.Data;
using Budget.Infrastructure.Data.Repositories;
using Budget.Infrastructure.Data.Seed;

using FluentValidation;

using MediatR;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Budget.API;

public static class StartupExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        return builder
            .AddDiServices()
            .AddPipelineServices()
            .AddSwaggerService()
            .Build();
    }

    public static WebApplication ConfigureApp(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Budget.API v1");
                c.OAuthClientId("budget-swagger");
                c.OAuthClientSecret("secret");
                c.OAuthScopes("budget-api");
            });
        }

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

    private static WebApplicationBuilder AddDiServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<BudgetContext>(options =>
            options.UseSqlServer("Name=BudgetConnection", sqlOptions =>
                sqlOptions.MigrationsAssembly("Budget.Infrastructure")));

        builder.Services.AddScoped<ISeeder, DemoDataSeeder>();

        builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
        builder.Services.AddScoped<IIncomeRepository, IncomeRepository>();

        builder.Services.AddScoped<IBudgetSummaryService, BudgetSummaryService>();

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.Load("Budget.Application")))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        builder.Services.AddValidatorsFromAssembly(Assembly.Load("Budget.Application"));

        builder.Services.AddAutoMapper(Assembly.Load("Budget.Application"));

        return builder;
    }

    private static WebApplicationBuilder AddPipelineServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
            policy.RequireClaim("scope", "budget-api");
        }));

        builder.Services.AddControllers(c => c.SuppressAsyncSuffixInActionNames = false);

        return builder;
    }

    private static WebApplicationBuilder AddSwaggerService(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new() { Title = "Budget.API", Version = "v1" });

            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Scheme = "oauth2",
                Description = "OAuth2 Authorization",
                Flows = new OpenApiOAuthFlows
                {
                    ClientCredentials = new OpenApiOAuthFlow
                    {
                        TokenUrl = new Uri(builder.Configuration["Swagger:OAuth2:TokenUrl"]!),
                        Scopes =
                        {
                            ["budget-api"] = "Budget API"
                        }
                    }
                }
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme{
                        Name = "oauth2",
                        Reference = new OpenApiReference{
                            Id = "oauth2",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return builder;
    }
}