using Microsoft.AspNetCore.SignalR;

namespace MircoLog.Lama.Server.Workers;

public interface IStatisticsHubClient
{
    Task ReceiveDailyStatistics(DailyStatistics statistics);
}

public class StatisticsHub : Hub<IStatisticsHubClient>
{
}