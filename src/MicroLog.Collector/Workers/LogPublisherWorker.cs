namespace MicroLog.Collector.Workers;

public class LogPublisherWorker : ConnectionBackgroundService
{
    public ILogPublisher _LogPublisher { get; set; }

    public LogPublisherWorker(ILogPublisher publishers)
    {
        _LogPublisher = publishers;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        GetConnectionPolicy().Execute(() =>
        {
            _LogPublisher.Connect();
        });
        return Task.CompletedTask;
    }
}
