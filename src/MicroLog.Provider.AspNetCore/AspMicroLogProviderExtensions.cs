using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Provider.AspNetCore
{
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
}
