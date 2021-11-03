using MicroLog.Collector.Config;
using MicroLog.Collector.RabbitMq;
using MicroLog.Collector.RabbitMq.Config;
using MicroLog.Collector.Utils;
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
            services.Configure<SinksConfig>(configuration.GetSection("Sinks"));

            var sinksConfig = services
                .BuildServiceProvider()
                .GetRequiredService<IOptions<SinksConfig>>()
                .Value;

            foreach (var mongoSink in sinksConfig.Mongo.OrEmptyIfNull())
            {
                services.AddSingleton<ILogSink>(new MongoLogRepository(mongoSink));
            }

            return services;
        }

        public static IServiceCollection AddCollector(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitCollectorConfig>(
                configuration
                    .GetSection("Collector")
                    .GetSection("RabbitMq"));

            return services;
        }

        public static IServiceCollection AddPublisher(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<PublisherConfig>(configuration.GetSection("Publisher"));

            var publisherConfig = services
                .BuildServiceProvider()
                .GetRequiredService<IOptions<PublisherConfig>>()
                .Value;

            if (publisherConfig is not null)
            {
                var rabbitConfig = services
                    .BuildServiceProvider()
                    .GetRequiredService<IOptions<RabbitCollectorConfig>>()
                    .Value;
                services.AddSingleton<ILogPublisher>(new RabbitLogPublisher(rabbitConfig, publisherConfig));
            }

            return services;
        }

        public static IServiceCollection AddConsumers(this IServiceCollection services)
        {
            var sinks = services
                .BuildServiceProvider()
                .GetServices<ILogSink>();

            foreach (var sink in sinks)
            {
                var rabbitConfig = services
                    .BuildServiceProvider()
                    .GetRequiredService<IOptions<RabbitCollectorConfig>>();
                services.AddSingleton<ILogConsumer>(new RabbitLogConsumer(rabbitConfig, sink));
            }

            if (sinks.Count() > 0)
            {
                services.AddHostedService<LogConsumerWorker>();
            }

            return services;
        }
    }
}
