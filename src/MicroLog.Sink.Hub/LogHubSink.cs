using Microsoft.AspNetCore.SignalR.Client;
using Polly;

namespace MicroLog.Sink.Hub;

public class LogHubSink : ILogSink, IAsyncDisposable
{
    public ISinkConfig Config { get; }
    private HubConfig _HubConfig { get; set; }
    private HubConnection _HubConnection { get; set; }
    public bool IsConnected => _HubConnection?.State == HubConnectionState.Connected;

    public LogHubSink(IOptions<HubConfig> config)
    {
        Config = _HubConfig = config.Value;
        _ = ConnectAsync();
    }

    private async Task ConnectAsync()
    {
        _HubConnection = new HubConnectionBuilder()
            .WithUrl(new Uri(_HubConfig.Url))
            .Build();

        await GetConnectionPolicy().ExecuteAsync(async () =>
        {
            await _HubConnection.StartAsync();
        });
    }

    public async Task InsertAsync(ILogEvent logEvent)
    {
        if (IsConnected)
        {
            var log = new LogEvent(logEvent);
            await _HubConnection.SendAsync("Insert", log);
        }
    }

    public async Task InsertAsync(IEnumerable<ILogEvent> logEvents)
    {
        if (IsConnected)
        {
            List<LogEvent> logs = logEvents.Select(log => new LogEvent(log)).ToList();
            await _HubConnection.SendAsync("InsertMany", logs);
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_HubConnection is not null)
        {
            await _HubConnection.DisposeAsync();
        }
    }

    private IAsyncPolicy GetConnectionPolicy()
    {
        var pauseBetweenFailures = TimeSpan.FromSeconds(30);
        var retryPolicy = Policy
             .Handle<Exception>()
             .WaitAndRetryForeverAsync(_ => pauseBetweenFailures);
        return retryPolicy;
    }
}