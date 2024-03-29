﻿using MicroLog.Collector.RabbitMq.Config;

namespace MicroLog.Collector.RabbitMq;

/// <summary>
/// Implementation of <see cref="ILogConsumer"/> with RabbitMq. 
/// </summary>
public class RabbitLogConsumer : RabbitLogBase, ILogConsumer
{
    private string _Queue { get; }
    private ILogSink _Sink { get; }
    private IConnection _Connection { get; set; }
    private IModel _Channel { get; set; }
    private ILogEnricher _SourceEnricher { get; }

    public RabbitLogConsumer(IOptions<RabbitCollectorConfig> rabbitConfig, ILogSink sink)
        : base(rabbitConfig.Value)
    {
        _Sink = sink;
        _Queue = $"log-{sink.Config.Name}";
        _SourceEnricher = new ValueEnricher("Source", RabbitConfig.HostName);
    }

    public void Consume()
    {
        _Connection = ConnectionFactory.CreateConnection();
        _Channel = _Connection.CreateModel();

        DeclareQueue(_Channel, _Queue);

        var consumer = new EventingBasicConsumer(_Channel);
        consumer.Received += async (sender, e) =>
        {
            try
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var log = JsonSerializer.Deserialize<LogEvent>(message);

                _SourceEnricher.Enrich(log);
                await _Sink.InsertAsync(log);

                _Channel.BasicAck(e.DeliveryTag, false);
            }
            catch (Exception)
            {
                _Channel.BasicNack(e.DeliveryTag, false, true);
            }
        };

        _Channel.BasicQos(0, 1000, false);
        _Channel.BasicConsume(_Queue, false, consumer);
    }

    public void Dispose()
    {
        _Connection.Close();
        _Connection.Dispose();
    }
}