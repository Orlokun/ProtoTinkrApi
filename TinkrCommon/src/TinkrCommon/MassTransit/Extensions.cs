using System;
using System.Reflection;
using GreenPipes;
using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using TinkrCommon.Settings;


namespace TinkrCommon.MassTransit
{
    public static class Extensions
    {
        public static IServiceCollection AddMassTransitWithRabbitMQ(this IServiceCollection services)
        {
            services.AddMassTransit(configure =>
            {
                configure.AddConsumers(Assembly.GetEntryAssembly());
                configure.UsingRabbitMq((context, configurator) =>
                {
                    var configuration = context.GetService<IConfiguration>();
                    var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
                    var rabbitMqSettings = configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
                    configurator.Host(rabbitMqSettings.Host);
                    configurator.ConfigureEndpoints(context,
                        new KebabCaseEndpointNameFormatter(serviceSettings.ServiceName, false));
                    configurator.UseMessageRetry(retryConfigurator =>
                    {
                        retryConfigurator.Interval(3, TimeSpan.FromSeconds(5));
                    });
                });
            });
            services.AddMassTransitHostedService();
            return services;
        }
    }
}