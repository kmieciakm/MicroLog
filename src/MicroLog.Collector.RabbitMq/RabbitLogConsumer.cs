using MicroLog.Collector.RabbitMq.Config;
using MicroLog.Core;
using MicroLog.Core.Abstractions;
using MicroLog.Core.Enrichers;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MicroLog.Collector.RabbitMq
{
    /// <summary>
    /// Implementation of <see cref="ILogConsumer"/> with RabbitMq. 
    /// </summary>
    public class RabbitLogConsumer : RabbitLogBase, ILogConsumer
    {
        private string _Queue { get; }
        private ILogSink _Sink { get; }
        private IConnection _Connection { get; set; }
        private IModel _Channel { get; set; }
        private ILogEnricher _SourceEnricher { get; }

        public RabbitLogConsumer(IOptions<RabbitCollectorConfig> rabbitConfig, ILogSink sink)
            : base(rabbitConfig.Value)
        {
            _Sink = sink;
            _Queue = $"log-{sink.Config.Name}";
            _SourceEnricher = new ValueEnricher("Source", RabbitConfig.HostName);
        }

        public void Consume()
        {
            _Connection = ConnectionFactory.CreateConnection();
            _Channel = _Connection.CreateModel();

            DeclareQueue(_Channel, _Queue);

            var consumer = new EventingBasicConsumer(_Channel);
            consumer.Received += async (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                LogEvent log = JsonSerializer.Deserialize<LogEvent>(message);
                _SourceEnricher.Enrich(log);
                await _Sink.InsertAsync(log);
            };

            _Channel.BasicConsume(_Queue, true, consumer);
        }

        public void Dispose()
        {
            _Connection.Close();
            _Connection.Dispose();
        }
    }
}
