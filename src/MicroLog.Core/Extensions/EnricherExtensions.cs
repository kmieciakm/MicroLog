using MicroLog.Core.Abstractions;
using MicroLog.Core.Enrichers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MicroLog.Core.Extensions;

public static class EnricherExtensions
{
    public static void UseHttpContextEnricher(this IApplicationBuilder app)
    {
        app.UseMiddleware<HttpContextEnricherMiddleware>();
    }

    public static void UseEnvironmentEnricher(this IApplicationBuilder app)
    {
        app.UseMiddleware<EnvironmentEnricherMiddleware>();
    }

    public static void UseEnvironmentEnricher(this IServiceProvider serviceProvider)
    {
        var logger = serviceProvider.GetRequiredService<ILogger>();
        var enricher = new EnvironmentEnricher();
        logger.AddEnricher(enricher);
    }

    public static void UseEnricher<T>(this IServiceProvider serviceProvider)
    {
        var logger = serviceProvider.GetRequiredService<ILogger>();
        var enricher = serviceProvider.GetRequiredService<T>() as ILogEnricher;
        logger.AddEnricher(enricher);
    }
}
