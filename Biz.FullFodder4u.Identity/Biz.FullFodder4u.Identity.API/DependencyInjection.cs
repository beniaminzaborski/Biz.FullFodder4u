using Microsoft.AspNetCore.Mvc.ApplicationModels;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Biz.FullFodder4u.Identity.API;

public static class DependencyInjection
{
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
