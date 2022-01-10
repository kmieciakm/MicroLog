using Microsoft.AspNetCore.SignalR;
using System.Timers;

namespace MircoLog.Lama.Server.Workers;

public class StatisticsSender : BackgroundService
{
    private const int SENDING_INTERVAL = 10000;
    private Timer _Timer { get; } = new Timer();

    public StatisticsSender(
        ILogStatsProvider statsProvider,
        IHubContext<StatisticsHub, IStatisticsHubClient> statisticsHub)
    {
        _Timer.Interval = SENDING_INTERVAL;
        _Timer.Elapsed += async (s, e) =>
        {
            var daily = statsProvider.GetDailyStatistics();
            var total = statsProvider.GetTotalStatistics();
            await statisticsHub.Clients.All.ReceiveDailyStatistics(daily, total);
        };
        _Timer.Start();
    }

    protected override Task ExecuteAsync(System.Threading.CancellationToken stoppingToken)
    {
        _Timer.Start();
        return Task.CompletedTask;
    }
}