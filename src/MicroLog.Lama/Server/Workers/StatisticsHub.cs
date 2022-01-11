using Microsoft.AspNetCore.SignalR;

namespace MircoLog.Lama.Server.Workers;

public interface IStatisticsHubClient
{
    Task ReceiveDailyStatistics(DailyStatistics daily, TotalStatistics total);
}

public class StatisticsHub : Hub<IStatisticsHubClient>
{
}