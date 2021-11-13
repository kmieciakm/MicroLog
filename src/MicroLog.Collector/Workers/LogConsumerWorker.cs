namespace MicroLog.Collector.Workers;

public class LogConsumerWorker : ConnectionBackgroundService
{
    public IEnumerable<ILogConsumer> _LogConsumers { get; set; }

    public LogConsumerWorker(IEnumerable<ILogConsumer> consumers)
    {
        _LogConsumers = consumers;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        foreach (var consumer in _LogConsumers)
        {
            GetConnectionPolicy().Execute(() =>
            {
                consumer.Consume();
            });
        }
        return Task.CompletedTask;
    }
}