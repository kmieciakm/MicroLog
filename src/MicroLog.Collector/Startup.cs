using MicroLog.Collector.RabbitMq;
using MicroLog.Collector.RabbitMq.Config;
using MicroLog.Collector.Workers;
using MicroLog.Core.Abstractions;
using MicroLog.Sink.MongoDb;
using MicroLog.Sink.MongoDb.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroLog.Collector
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions<MongoSinkConfig>("MongoSinkConfig");
            services.AddSingleton<ILogSink, MongoLogRepository>();

            services.AddOptions<RabbitCollectorConfig>("RabbitCollectorConfig");
            services.AddSingleton<ILogPublisher, RabbitLogPublisher>();
            services.AddSingleton<ILogConsumer, RabbitLogConsumer>();

            services.AddHostedService<LogProcessor>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MicroLog.Collector", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MicroLog.Collector v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
