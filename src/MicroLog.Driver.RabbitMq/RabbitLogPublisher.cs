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
    public class RabbitLogPublisher : ILogSink
    {
        private ConnectionFactory _ConnectionFactory { get; set; }
        private string _QueueName { get; set; }

        public RabbitLogPublisher(RabbitLogConfig rabbitConfig)
        {
            _ConnectionFactory = new ConnectionFactory
            {
                HostName = rabbitConfig.HostName,
                Port = rabbitConfig.Port,
                UserName = rabbitConfig.UserName,
                Password = rabbitConfig.Password
            };
            _QueueName = rabbitConfig.Queue;
        }

        public Task InsertAsync(ILogEvent logEntity)
        {
            using (IConnection connection = _ConnectionFactory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: _QueueName,
                        durable: true,
                        exclusive: false,
                        autoDelete: false);

                    var body = JsonSerializer.SerializeToUtf8Bytes(logEntity);
                    channel.BasicPublish("", _QueueName, null, body);
                }
            }
            return Task.CompletedTask;
        }

        public Task InsertAsync(IEnumerable<ILogEvent> logEntities)
        {
            using (IConnection connection = _ConnectionFactory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: _QueueName,
                        durable: true,
                        exclusive: false,
                        autoDelete: false);

                    ReadOnlyMemory<byte> body;
                    var basicPublishBatch = channel.CreateBasicPublishBatch();
                    foreach (var log in logEntities)
                    {
                        body = JsonSerializer.SerializeToUtf8Bytes(log);
                        basicPublishBatch.Add("", _QueueName, false, null, body);
                    }
                    
                    basicPublishBatch.Publish();
                }
            }
            return Task.CompletedTask;
        }
    }
}
