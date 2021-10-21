using MicroLog.Core.Abstractions;
using MicroLog.Driver.RabbitMq.Config;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MicroLog.Driver.RabbitMq
{
    public class RabbitLogPublisher : RabbitLogBase, ILogCollector
    {
        public RabbitLogPublisher(RabbitLogConfig rabbitConfig)
            : base(rabbitConfig)
        {
        }

        public Task InsertAsync(ILogEvent logEntity)
        {
            using (IConnection connection = ConnectionFactory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    DeclareQueue(channel);

                    var prop = GetProperties(channel, logEntity);
                    var body = JsonSerializer.SerializeToUtf8Bytes(logEntity);
                    channel.BasicPublish("", QueueName, prop, body);
                }
            }
            return Task.CompletedTask;
        }

        public Task InsertAsync(IEnumerable<ILogEvent> logEntities)
        {
            using (IConnection connection = ConnectionFactory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    DeclareQueue(channel);

                    ReadOnlyMemory<byte> body;
                    IBasicProperties prop;
                    IBasicPublishBatch basicPublishBatch = channel.CreateBasicPublishBatch();

                    foreach (var log in logEntities)
                    {
                        prop = GetProperties(channel, log);
                        body = JsonSerializer.SerializeToUtf8Bytes(log);
                        basicPublishBatch.Add("", QueueName, false, prop, body);
                    }

                    basicPublishBatch.Publish();
                }
            }
            return Task.CompletedTask;
        }
    }
}
