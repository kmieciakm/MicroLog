using MicroLog.Collector.RabbitMq.Config;
using MicroLog.Core.Abstractions;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Collector.RabbitMq
{
    public abstract class RabbitLogBase
    {
        protected ConnectionFactory ConnectionFactory { get; set; }
        protected string QueueName { get; set; }

        public RabbitLogBase(RabbitCollectorConfig rabbitConfig)
        {
            ConnectionFactory = new ConnectionFactory
            {
                HostName = rabbitConfig.HostName,
                Port = rabbitConfig.Port,
                UserName = rabbitConfig.UserName,
                Password = rabbitConfig.Password
            };
            QueueName = rabbitConfig.Queue;
        }

        protected void DeclareQueue(IModel channel)
        {
            var args = new Dictionary<string, object>
                    {
                        { " x-max-priority ", 4 }
                    };

            channel.QueueDeclare(
                queue: QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: args);
        }

        protected IBasicProperties GetProperties(IModel channel, ILogEvent logEntity)
        {
            var properties = channel.CreateBasicProperties();
            properties.Priority = logEntity.Level switch
            {
                Core.LogLevel.None => 1,
                Core.LogLevel.Trace => 1,
                Core.LogLevel.Debug => 1,
                Core.LogLevel.Information => 1,
                Core.LogLevel.Warning => 2,
                Core.LogLevel.Error => 3,
                Core.LogLevel.Critical => 4,
                _ => 1
            };
            return properties;
        }
    }
}
