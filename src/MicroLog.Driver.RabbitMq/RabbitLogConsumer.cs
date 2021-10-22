using MicroLog.Collector.RabbitMq.Config;
using MicroLog.Core.Abstractions;
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
    public class RabbitLogConsumer : RabbitLogBase, ILogConsumer
    {
        private IEnumerable<ILogSink> _Sinks { get; set; }

        public RabbitLogConsumer(RabbitCollectorConfig rabbitConfig, IEnumerable<ILogSink> sinks)
            : base(rabbitConfig)
        {
            _Sinks = sinks;
        }

        public void Consume()
        {
            using (IConnection connection = ConnectionFactory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    DeclareQueue(channel);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += async (sender, e) =>
                    {
                        var body = e.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        ILogEvent log = JsonSerializer.Deserialize<ILogEvent>(message);
                        foreach (var sink in _Sinks)
                        {
                            await sink.InsertAsync(log);
                        }
                    };
                    channel.BasicConsume(QueueName, true, consumer);
                }
            }
        }
    }
}
