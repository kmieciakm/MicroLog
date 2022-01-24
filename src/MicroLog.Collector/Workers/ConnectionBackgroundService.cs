using Polly;

namespace MicroLog.Collector.Workers;

public abstract class ConnectionBackgroundService : BackgroundService
{
    protected ISyncPolicy GetConnectionPolicy()
    {
        var maxNumberOfRetry = 5;
        var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetry(maxNumberOfRetry, (attemptCount) =>
                    TimeSpan.FromSeconds(attemptCount * 2));
        return retryPolicy;
    }
}
