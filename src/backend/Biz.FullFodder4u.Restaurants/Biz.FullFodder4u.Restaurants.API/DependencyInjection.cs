using Biz.FullFodder4u.Restaurants.API.Infrastructure;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Reflection;
using System.Text;

namespace Biz.FullFodder4u.Restaurants.API;

public static class DependencyInjection
{
    public static IServiceCollection AddDbMigrator(this IServiceCollection services, string connectionString)
    {
        return services
            .AddLogging(c => c.AddFluentMigratorConsole())
            .AddFluentMigratorCore()
            .ConfigureRunner(options =>
            {
                options
                    .AddPostgres()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(Assembly.GetExecutingAssembly()).For.All();
            });
    }

    public static IServiceCollection AddCustomControllers(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.SuppressAsyncSuffixInActionNames = false;
            options.Conventions.Add(
                new RouteTokenTransformerConvention(
                    new SlugifyParameterTransformer()));
        });

        return services;
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidIssuer = configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
            });

        return services;
    }

    public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
    {
        return services.AddSwaggerGen(options =>
        {
            var jwtSecurityRequirements = new OpenApiSecurityRequirement();
            jwtSecurityRequirements.Add(new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            }, new string[] { });
            options.AddSecurityRequirement(jwtSecurityRequirements);
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() { In = ParameterLocation.Header, Description = "Please insert token with Bearer into field", Name = "Authorization", Type = SecuritySchemeType.ApiKey });
        });

    }

    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEntityFrameworkNpgsql()
            .AddDbContext<ApplicationContext>(
                options => options.UseNpgsql(
                configuration.GetConnectionString("RestaurantsApi")));

        services.AddDbMigrator(
            configuration.GetConnectionString("RestaurantsApi"));

        return services;
    }

    public static IServiceCollection AddOpenTelemetry(this IServiceCollection services, string serviceName, IConfiguration configuration)
    {
        services.AddOpenTelemetryTracing(options =>
        {
            options
                .AddConsoleExporter()
                .AddJaegerExporter(jaegerOptions =>
                {
                    jaegerOptions.AgentHost = configuration["Jaeger:AgentHost"];
                    jaegerOptions.AgentPort = configuration.GetValue<int>("Jaeger:AgentPort");
                })
                .AddSource(serviceName)
                .SetResourceBuilder(
                    ResourceBuilder.CreateDefault()
                        .AddService(serviceName: serviceName, serviceVersion: "1.0.0.0"))
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddEntityFrameworkCoreInstrumentation(o => o.SetDbStatementForText = true)
                .AddSqlClientInstrumentation();
        });

        services
            .AddSingleton(TracerProvider.Default.GetTracer(serviceName));

        return services;
    }
}
