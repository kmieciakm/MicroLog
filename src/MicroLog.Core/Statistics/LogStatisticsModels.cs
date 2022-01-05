namespace MicroLog.Core.Statistics;

public record struct LogsStatistics(
    IEnumerable<LogsCount> LogsCount,
    long TotalCount
);

public record struct LogsCount(
    LogLevel Level,
    long Count
);