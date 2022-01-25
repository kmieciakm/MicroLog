using MicroLog.Collector.Client;
using MicroLog.Core.Extensions;
using MicroLog.Provider.AspNetCore;
using MicroShop.Core;
using MicroShop.Shipping;
using IMicroLogger = MicroLog.Core.Abstractions.ILogger;

IHostBuilder builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((hostContext, services) =>
{
    services.Configure<ProcessingConfig>(
        hostContext.Configuration.GetSection("ProcessingConfig"));

    services
        .Configure<LogCollectorConfig>(
            hostContext.Configuration.GetSection("LogCollectorConfig"))
        .AddSingleton<IMicroLogger, LogCollectorClient>()
        .AddLogging(builder => builder
            .ClearProviders()
            .AddAspMicroLogger());

    services.AddHostedService<Worker>();
});

var host = builder.Build();

host.Services.UseEnvironmentEnricher();

await host.RunAsync();