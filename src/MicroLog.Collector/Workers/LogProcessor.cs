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
        public ILogConsumer _LogConsumer { get; set; }

        public LogProcessor(ILogConsumer consumer)
        {
            _LogConsumer = consumer;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _LogConsumer.Consume();
            return Task.CompletedTask;
        }
    }
}
