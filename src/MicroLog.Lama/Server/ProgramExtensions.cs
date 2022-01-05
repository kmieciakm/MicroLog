namespace MircoLog.Lama.Server;

public static class ProgramExtensions
{
    public static IServiceCollection AddLogRegistry(this IServiceCollection services, IConfiguration configuration)
    {
        MongoLogRepository registry = GetMongoRegistry(services, configuration);
        services.AddSingleton<ILogRegistry>(registry);
        return services;
    }

    public static IServiceCollection AddLogStatsProvider(this IServiceCollection services, IConfiguration configuration)
    {
        MongoLogRepository registry = GetMongoRegistry(services, configuration);
        services.AddSingleton<ILogStatsProvider>(registry);
        return services;
    }

    private static MongoLogRepository GetMongoRegistry(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoConfig>(configuration.GetSection("Registry"));

        var registryConfig = services
            .BuildServiceProvider()
            .GetRequiredService<IOptions<MongoConfig>>()
            .Value;

        var registry = new MongoLogRepository(registryConfig);
        return registry;
    }
}
