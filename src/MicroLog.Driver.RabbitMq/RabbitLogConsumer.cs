using MicroLog.Core.Abstractions;
using MicroLog.Driver.RabbitMq.Config;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Driver.RabbitMq
{
    public class RabbitLogConsumer : RabbitLogBase, ILogConsumer
    {
        public RabbitLogConsumer(RabbitLogConfig rabbitConfig)
            : base(rabbitConfig)
        {
        }

        public void Consume()
        {
            using (IConnection connection = ConnectionFactory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    DeclareQueue(channel);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (sender, e) =>
                    {
                        
                    };
                    channel.BasicConsume(QueueName, true, consumer);
                }
            }
        }
    }
}
