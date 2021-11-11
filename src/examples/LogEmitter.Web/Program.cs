using MicroLog.Collector.Client;
using MicroLog.Provider.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMicroLogger = MicroLog.Core.Abstractions.ILogger;

namespace LogEmitter.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    /*
                     * Integration of MicroLog with default .Net ILogger interface.
                     */
                    //webBuilder.ConfigureServices(services =>
                    //{
                    //    var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
                    //    services.Configure<LogCollectorConfig>(config.GetSection("LogCollectorConfig"));
                    //    services.AddSingleton<IMicroLogger, LogCollectorClient>();
                    //});

                    //webBuilder.ConfigureLogging(builder => builder
                    //    .ClearProviders()
                    //    .AddAspMicroLogger());
                    /*
                     * --------------------------------------------------------------
                     */

                    webBuilder.UseStartup<Startup>();
                });
    }
}