using Microsoft.AspNetCore.SignalR;
using MircoLog.Lama.Shared;

namespace MircoLog.Lama.Server.Hubs;

public interface ILogHub
{
    Task SendLog(LogMessage message);
}

public interface ILogHubClient
{
    Task ReceiveLog(LogMessage log);
}

public class LogHub : Hub<ILogHubClient>, ILogHub
{
    public async Task SendLog(LogMessage message)
    {
        await SendMessage(message);
    }

    public async Task SendMessage(LogMessage message)
    {
        await Clients.All.ReceiveLog(message);
    }
}
