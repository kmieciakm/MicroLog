using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace MicroLog.Provider.AspNetCore;

public static class AspMicroLogProviderExtensions
{
    /// <summary>
    /// Adds the <see cref="AspMicroLoggerProvider"/> to logging builder.
    /// </summary>
    /// <param name="builder">Logging builder.</param>
    /// <returns>Logging builder.</returns>
    public static ILoggingBuilder AddAspMicroLogger(this ILoggingBuilder builder)
    {
        builder.AddConfiguration();

        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Singleton<ILoggerProvider, AspMicroLoggerProvider>());

        return builder;
    }
}