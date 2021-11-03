using MicroLog.Collector.Config;
using MicroLog.Collector.RabbitMq;
using MicroLog.Collector.RabbitMq.Config;
using MicroLog.Collector.Workers;
using MicroLog.Core.Abstractions;
using MicroLog.Sink.MongoDb;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroLog.Collector
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSinks(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<Configs>(configuration.GetSection("Sinks"));

            var sinksConfig = services
                .BuildServiceProvider()
                .GetRequiredService<IOptions<Configs>>()
                .Value;

            foreach (var mongoSink in sinksConfig.Mongo)
            {
                services.AddSingleton<ILogSink>(new MongoLogRepository(mongoSink));
            }

            return services;
        }

        public static IServiceCollection AddCollectors(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitCollectorConfig>(
                configuration
                    .GetSection("Collector")
                    .GetSection("RabbitMq"));

            return services;
        }

        public static IServiceCollection AddPublishers(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<PublishersConfig>(configuration.GetSection("Publisher"));

            var rabbitConfig = services
                .BuildServiceProvider()
                .GetRequiredService<IOptions<RabbitCollectorConfig>>()
                .Value;

            var publisherConfig = services
                .BuildServiceProvider()
                .GetRequiredService<IOptions<PublishersConfig>>()
                .Value;

            if (publisherConfig.RabbitMq is not null)
            {
                services.AddSingleton<ILogPublisher>(new RabbitLogPublisher(rabbitConfig, publisherConfig.RabbitMq));
            }

            return services;
        }

        public static IServiceCollection AddConsumers(this IServiceCollection services)
        {
            services.AddSingleton<ILogConsumer, RabbitLogConsumer>();
            services.AddHostedService<LogProcessor>();

            return services;
        }
    }
}
