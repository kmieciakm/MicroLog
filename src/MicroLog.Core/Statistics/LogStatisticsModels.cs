namespace MicroLog.Core.Statistics;

public class LogsStatistics
{
    public IEnumerable<LogsCount> LogsCount { get; set; }
    public long TotalCount { get; set; }
};

public record struct LogsCount(
    LogLevel Level,
    long Count
);