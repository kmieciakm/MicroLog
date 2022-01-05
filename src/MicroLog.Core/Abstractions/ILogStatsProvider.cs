using MicroLog.Core.Statistics;

namespace MicroLog.Core.Abstractions;

/// <summary>
/// Provides statistics about stored logs.
/// </summary>
public interface ILogStatsProvider
{
    LogsStatistics GetDailyStatistics();
    LogsStatistics GetTotalStatistics();
}