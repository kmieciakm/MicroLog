using Microsoft.AspNetCore.SignalR;

namespace MicroLog.Sink.Hub;

public class LogHub : Hub<ILogHubClient>
{
    [HubMethodName("Insert")]
    public async Task InsertAsync(LogEvent logEvent)
    {
        await BroadcastLog(logEvent);
    }

    [HubMethodName("InsertMany")]
    public async Task InsertAsync(List<LogEvent> logEvents)
    {
        foreach (var logEvent in logEvents)
        {
            await BroadcastLog(logEvent);
        }
    }

    protected async Task BroadcastLog(LogEvent logEvent)
    {
        await Clients.All.ReceiveLog(logEvent);
    }
}