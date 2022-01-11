using MicroLog.Core.Statistics;

namespace MicroLog.Core.Abstractions;

/// <summary>
/// Provides statistics about stored logs.
/// </summary>
public interface ILogStatsProvider
{
    DailyStatistics GetDailyStatistics();
    TotalStatistics GetTotalStatistics();
}