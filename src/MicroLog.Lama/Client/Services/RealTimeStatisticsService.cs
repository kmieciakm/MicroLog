using MicroLog.Core.Statistics;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System;

namespace MircoLog.Lama.Client.Services;

interface IRealTimeStatisticsService : IRealTimeService
{
    event StatisticsDelegate OnStatisticsRefreshed;
}

class RealTimeStatisticsService : RealTimeService, IRealTimeStatisticsService
{
    public event StatisticsDelegate OnStatisticsRefreshed;

    public RealTimeStatisticsService(NavigationManager navigationManager)
        : base(navigationManager.ToAbsoluteUri("/hub/statistics"))
    {
        HubConnection.On<DailyStatistics>("ReceiveDailyStatistics", (stats) =>
        {
            OnStatisticsRefreshed?.Invoke(new StatisticsArgs(stats));
        });
    }
}

public delegate void StatisticsDelegate(StatisticsArgs arg);

public class StatisticsArgs : EventArgs
{
    public DailyStatistics DailyStatistics { get; }

    public StatisticsArgs(DailyStatistics stats)
    {
        DailyStatistics = stats;
    }
}
