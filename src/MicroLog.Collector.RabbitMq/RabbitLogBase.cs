using MicroLog.Collector.RabbitMq.Config;

namespace MicroLog.Collector.RabbitMq;

/// <summary>
/// Base class used to connect and configure Rabbit Message Queue.
/// </summary>
public abstract class RabbitLogBase
{
    protected RabbitCollectorConfig RabbitConfig { get; set; }
    protected ConnectionFactory ConnectionFactory { get; set; }

    public RabbitLogBase(RabbitCollectorConfig rabbitConfig)
    {
        RabbitConfig = rabbitConfig;
        ConnectionFactory = new ConnectionFactory
        {
            HostName = rabbitConfig.HostName,
            Port = rabbitConfig.Port,
            UserName = rabbitConfig.UserName,
            Password = rabbitConfig.Password
        };
    }

    /// <summary>
    /// Declares a queue with priority.
    /// </summary>
    /// <param name="channel">AMQP channel.</param>
    /// <param name="queue">Queue name.</param>
    protected void DeclareQueue(IModel channel, string queue)
    {
        var args = new Dictionary<string, object>
                {
                    { " x-max-priority ", 4 }
                };

        channel.QueueDeclare(
            queue: queue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: args);
    }

    /// <summary>
    /// Creates log specific properties for the channel.
    /// Sets the priority of message based on log level (<see cref="LogLevel"/>) of a given log.
    /// </summary>
    /// <param name="channel">AMQP channel.</param>s
    /// <param name="logEvent">Log event.</param>
    /// <returns>Created properties.</returns>
    protected IBasicProperties GetProperties(IModel channel, ILogEvent logEvent)
    {
        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;
        properties.Priority = logEvent.Level switch
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
