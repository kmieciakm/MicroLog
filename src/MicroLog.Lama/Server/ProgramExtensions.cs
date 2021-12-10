using MicroLog.Sink.MongoDb;
using MicroLog.Sink.MongoDb.Config;

namespace MircoLog.Lama.Server;

public static class ProgramExtensions
{
    public static IServiceCollection AddLogRegistry(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoConfig>(configuration.GetSection("Registry"));

        var registryConfig = services
            .BuildServiceProvider()
            .GetRequiredService<IOptions<MongoConfig>>()
            .Value;

        services.AddSingleton<ILogRegistry>(new MongoLogRepository(registryConfig));
        return services;
    }
}
