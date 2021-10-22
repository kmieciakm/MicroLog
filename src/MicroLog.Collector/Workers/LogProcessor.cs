using MicroLog.Core.Abstractions;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MicroLog.Collector.Workers
{
    public class LogProcessor : BackgroundService
    {
        private System.Timers.Timer _Timer { get; }
        public ILogConsumer _LogConsumer { get; set; }

        public LogProcessor(ILogConsumer consumer)
        {
            _LogConsumer = consumer;
            _Timer = new System.Timers.Timer(15000);
            _Timer.Elapsed += (sender, e) =>
            {
                _LogConsumer.Consume();
            };
            _Timer.AutoReset = false;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _Timer.Start();
            return Task.CompletedTask;
        }
    }
}
