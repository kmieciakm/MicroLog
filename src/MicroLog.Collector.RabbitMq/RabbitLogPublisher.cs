using MicroLog.Collector.RabbitMq.Config;

namespace MicroLog.Collector.RabbitMq
{
    /// <summary>
    /// Implementation of <see cref="ILogPublisher"/> with RabbitMq. 
    /// </summary>
    public class RabbitLogPublisher : RabbitLogBase, ILogPublisher
    {
        public IPublisherConfig Config { get; }
        private IEnumerable<string> Queues { get; }

        public RabbitLogPublisher(RabbitCollectorConfig rabbitConfig, IPublisherConfig publisherConfig)
            : base(rabbitConfig)
        {
            Config = publisherConfig;
            Queues = publisherConfig.GetQueues();
        }

        public RabbitLogPublisher(IOptions<RabbitCollectorConfig> rabbitOptions, IPublisherConfig publisherOptions)
            : this(rabbitOptions.Value, publisherOptions)
        {
        }

        public Task PublishAsync(ILogEvent logEvent)
        {
            using (IConnection connection = ConnectionFactory.CreateConnection())
            {
                foreach (var queue in Queues)
                {
                    using (IModel channel = connection.CreateModel())
                    {
                        DeclareQueue(channel, queue);

                        var prop = GetProperties(channel, logEvent);
                        var body = JsonSerializer.SerializeToUtf8Bytes(logEvent);
                        channel.BasicPublish("", queue, prop, body);
                    }
                }
            }
            return Task.CompletedTask;
        }

        public Task PublishAsync(IEnumerable<ILogEvent> logEvents)
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

                        foreach (var log in logEvents)
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
