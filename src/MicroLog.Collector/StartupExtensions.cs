using MicroLog.Collector.Config;
using MicroLog.Collector.Middleware;
using MicroLog.Collector.Utils;
using MicroLog.Collector.Workers;

namespace MicroLog.Collector;

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

        foreach (var hubSinkConfig in sinksConfig.Hub.OrEmptyIfNull())
        {
            var hubOptions = Options.Create(hubSinkConfig);
            services.AddSingleton<ILogSink>(new LogHubSink(hubOptions));
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
            .GetService<IOptions<PublisherConfig>>()
            .Value;

        if (publisherConfig.IsPublisherSpecified)
        {
            var rabbitConfig = services
                .BuildServiceProvider()
                .GetRequiredService<IOptions<RabbitCollectorConfig>>()
                .Value;
            services.AddSingleton<ILogPublisher>(new RabbitLogPublisher(rabbitConfig, publisherConfig));
        }

        if (publisherConfig.IsPublisherSpecified)
        {
            services.AddHostedService<LogPublisherWorker>();
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

        if (sinks.Any())
        {
            services.AddHostedService<LogConsumerWorker>();
        }

        return services;
    }

    public static IApplicationBuilder UseRequestBroker(this IApplicationBuilder app)
        => app.UseMiddleware<CollectorRequestBroker>();
}