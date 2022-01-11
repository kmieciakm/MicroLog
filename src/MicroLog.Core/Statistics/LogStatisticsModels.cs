namespace MicroLog.Core.Statistics;

public class DailyStatistics : LogsStatistics
{
    public Dictionary<DateTime, int> LogsInterval { get; set; }
}

public class TotalStatistics : LogsStatistics
{
    public string ProviderName { get; set; }
}

public class LogsStatistics
{
    public IEnumerable<LogsCount> LogsCount { get; set; }
    public long TotalCount { get; set; }
};

public record struct LogsCount(
    LogLevel Level,
    long Count
);