﻿using Biz.FullFodder4u.Profiles.API.IntegrationEventHandlers;
using MassTransit;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Biz.FullFodder4u.Profiles.API;

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

    public static IServiceCollection AddCustomMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration["RabbitMq:Host"], configuration.GetValue<ushort>("RabbitMq:Port"), configuration["RabbitMq:VirtualHost"], h =>
                {
                    h.Username(configuration["RabbitMq:Username"]);
                    h.Password(configuration["RabbitMq:Password"]);
                });

                cfg.ReceiveEndpoint("user-events-listener", e =>
                {
                    e.Consumer<UserCreatedEventHandler>();
                });
            });
        })
        .AddMassTransitHostedService();

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
                .AddSqlClientInstrumentation()
                .AddMassTransitInstrumentation();
        });

        services
            .AddSingleton(TracerProvider.Default.GetTracer(serviceName));

        return services;
    }
}
