using MicroLog.Core.Abstractions;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MicroLog.Collector.Workers
{
    public class LogConsumerWorker : BackgroundService
    {
        private System.Timers.Timer _Timer { get; }
        public IEnumerable<ILogConsumer> _LogConsumers { get; set; }

        public LogConsumerWorker(IEnumerable<ILogConsumer> consumers)
        {
            _LogConsumers = consumers;
            _Timer = new System.Timers.Timer(15000);
            _Timer.Elapsed += (sender, e) =>
            {
                foreach(var consumer in _LogConsumers)
                    consumer.Consume();
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
