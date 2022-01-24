using MicroLog.Collector.RabbitMq.Config;

namespace MicroLog.Collector.RabbitMq;

/// <summary>
/// Implementation of <see cref="ILogPublisher"/> with RabbitMq.
/// </summary>
public class RabbitLogPublisher : RabbitLogBase, ILogPublisher
{
    public IPublisherConfig Config { get; }
    private List<(string queue, IModel channel)> _Channels { get; } = new();
    private IConnection _Connection { get; set; }

    public RabbitLogPublisher(RabbitCollectorConfig rabbitConfig, IPublisherConfig publisherConfig)
        : base(rabbitConfig)
    {
        Config = publisherConfig;
    }

    public RabbitLogPublisher(IOptions<RabbitCollectorConfig> rabbitOptions, IPublisherConfig publisherOptions)
        : this(rabbitOptions.Value, publisherOptions)
    {
    }

    public void Connect()
    {
        _Connection = ConnectionFactory.CreateConnection();
        foreach (var queue in Config.GetQueues())
        {
            var model = _Connection.CreateModel();
            DeclareQueue(model, queue);
            _Channels.Add(new (queue, model));
        }
    }

    public Task PublishAsync(ILogEvent logEvent)
    {
        foreach (var (queue, channel) in _Channels)
        {
            var prop = GetProperties(channel, logEvent);
            var body = JsonSerializer.SerializeToUtf8Bytes(logEvent);
            channel.BasicPublish("", queue, prop, body);
        }
        return Task.CompletedTask;
    }

    public Task PublishAsync(IEnumerable<ILogEvent> logEvents)
    {
        foreach (var (queue, channel) in _Channels)
        {
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
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _Connection.Close();
        _Connection.Dispose();
    }
}
