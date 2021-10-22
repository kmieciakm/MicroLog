using MicroLog.Collector.RabbitMq.Config;
using MicroLog.Core;
using MicroLog.Core.Abstractions;
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
    public class RabbitLogConsumer : RabbitLogBase, ILogConsumer, IDisposable
    {
        private IEnumerable<ILogSink> _Sinks { get; set; }
        private IConnection _Connection { get; set; }
        private IModel _Channel { get; set; }

        public RabbitLogConsumer(IOptions<RabbitCollectorConfig> rabbitConfig, IEnumerable<ILogSink> sinks)
            : base(rabbitConfig.Value)
        {
            _Sinks = sinks;
            _Connection = ConnectionFactory.CreateConnection();
            _Channel = _Connection.CreateModel();
        }

        public void Consume()
        {
            DeclareQueue(_Channel);

            var consumer = new EventingBasicConsumer(_Channel);
            consumer.Received += async (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                ILogEvent log = JsonSerializer.Deserialize<LogEvent>(message);
                foreach (var sink in _Sinks)
                {
                    await sink.InsertAsync(log);
                }
            };
            _Channel.BasicConsume(QueueName, true, consumer);
        }

        public void Dispose()
        {
            _Connection.Close();
            _Connection.Dispose();
        }
    }
}
