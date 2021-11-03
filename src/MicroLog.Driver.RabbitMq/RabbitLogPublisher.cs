using MicroLog.Collector.RabbitMq.Config;
using MicroLog.Core.Abstractions;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MicroLog.Collector.RabbitMq
{
    public class RabbitLogPublisher : RabbitLogBase, ILogPublisher
    {
        public IEnumerable<string> Queues { get; set; }

        public RabbitLogPublisher(RabbitCollectorConfig rabbitConfig, RabbitPublisherConfig rabbitPublisherConfig)
            : base(rabbitConfig)
        {
            Queues = rabbitPublisherConfig.GetQueues();
        }

        public RabbitLogPublisher(IOptions<RabbitCollectorConfig> rabbitOptions, IOptions<RabbitPublisherConfig> rabbitPublisherOptions)
            : this(rabbitOptions.Value, rabbitPublisherOptions.Value)
        {
        }

        public Task PublishAsync(ILogEvent logEntity)
        {
            using (IConnection connection = ConnectionFactory.CreateConnection())
            {
                foreach (var queue in Queues)
                {
                    using (IModel channel = connection.CreateModel())
                    {
                        DeclareQueue(channel, queue);

                        var prop = GetProperties(channel, logEntity);
                        var body = JsonSerializer.SerializeToUtf8Bytes(logEntity);
                        channel.BasicPublish("", queue, prop, body);
                    }
                }
            }
            return Task.CompletedTask;
        }

        public Task PublishAsync(IEnumerable<ILogEvent> logEntities)
        {
            using (IConnection connection = ConnectionFactory.CreateConnection())
            {
                foreach (var queue in Queues)
                {
                    using (IModel channel = connection.CreateModel())
                    {
                        DeclareQueue(channel, queue);

                        ReadOnlyMemory<byte> body;
                        IBasicProperties prop;
                        IBasicPublishBatch basicPublishBatch = channel.CreateBasicPublishBatch();

                        foreach (var log in logEntities)
                        {
                            prop = GetProperties(channel, log);
                            body = JsonSerializer.SerializeToUtf8Bytes(log);
                            basicPublishBatch.Add("", queue, false, prop, body);
                        }

                        basicPublishBatch.Publish();
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}
