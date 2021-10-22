using MicroLog.Collector.Config;
using MicroLog.Core.Abstractions;
using MicroLog.Sink.MongoDb;
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
        public static IServiceCollection RegisterSinks(this IServiceCollection services)
        {
            var sinksConfig = services
                .BuildServiceProvider()
                .GetRequiredService<IOptions<SinksConfig>>()
                .Value;

            foreach (var mongoSink in sinksConfig.Mongo)
            {
                services.AddSingleton<ILogSink>(new MongoLogRepository(mongoSink));
            }
            return services;
        }
    }
}
