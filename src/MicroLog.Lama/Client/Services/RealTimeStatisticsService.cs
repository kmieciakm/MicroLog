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
        HubConnection.On<DailyStatistics, TotalStatistics>("ReceiveDailyStatistics", (daily, total) =>
        {
            OnStatisticsRefreshed?.Invoke(new StatisticsArgs(daily, total));
        });
    }
}

public delegate void StatisticsDelegate(StatisticsArgs arg);

public class StatisticsArgs : EventArgs
{
    public DailyStatistics DailyStatistics { get; }
    public TotalStatistics TotalStatistics { get; }

    public StatisticsArgs(DailyStatistics daily, TotalStatistics total)
    {
        DailyStatistics = daily;
        TotalStatistics = total;
    }
}
