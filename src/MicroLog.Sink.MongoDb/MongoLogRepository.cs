using MicroLog.Core.Statistics;
using MicroLog.Sink.MongoDb.Config;
using Microsoft.Extensions.Options;

namespace MicroLog.Sink.MongoDb;

/// <summary>
/// A MongoDb data access point of logs.
/// </summary>
public class MongoLogRepository : ILogSink, ILogRegistry, ILogStatsProvider
{
    private const string COLLECTION_NAME = "logs";
    private const int MAX_CONNECTION_POOL_SIZE = 1000;

    public ISinkConfig Config { get; }
    private IMongoDatabase _Database { get; }
    private IMongoCollection<MongoLogEntity> _Collection { get; }

    public MongoLogRepository(IOptions<MongoConfig> configOptions)
        : this(configOptions.Value)
    {
    }

    public MongoLogRepository(MongoConfig config)
    {
        Config = config;
        var settings = MongoClientSettings.FromConnectionString(config.ConnectionString);
        settings.MaxConnectionPoolSize = MAX_CONNECTION_POOL_SIZE;
        var client = new MongoClient(settings);
        _Database = client.GetDatabase(config.DatabaseName);
        _Collection = _Database.GetCollection<MongoLogEntity>(COLLECTION_NAME);
    }

    async Task<PaginationResult<ILogEvent>> ILogRegistry.GetAsync(int skip, int take)
    {
        var cursor = _Collection.Find(_ => true);
        var totalEntities = await cursor.CountDocumentsAsync();
        var entities = cursor
            .Skip(skip)
            .Limit(take)
            .ToEnumerable();

        return new PaginationResult<ILogEvent>()
        {
            Items = entities,
            TotalCount = (int)totalEntities
        };
    }

    async Task<ILogEvent> ILogRegistry.GetAsync(ILogEventIdentity identity)
    {
        var id = new MongoLogIdentity(identity.EventId);
        using var entity = await _Collection.FindAsync(entity => entity.Identity.Equals(id));
        return entity.FirstOrDefault();
    }

    async Task<IEnumerable<ILogEvent>> ILogRegistry.GetAsync(IEnumerable<ILogEventIdentity> identities)
    {
        using var entity = await _Collection.FindAsync(entity => entity.Identity.Equals(identities));
        return entity.ToEnumerable();
    }

    async Task ILogSink.InsertAsync(ILogEvent logEvent)
    {
        var entity = MongoLogMapper.Map(logEvent);
        await _Collection.InsertOneAsync(entity);
    }

    async Task ILogSink.InsertAsync(IEnumerable<ILogEvent> logEvents)
    {
        var entities = logEvents.Select(entity => MongoLogMapper.Map(entity));
        await _Collection.InsertManyAsync(entities);
    }

    public DailyStatistics GetDailyStatistics()
    {
        (var totalCount, var logsCount) = GetDailyLogCount();
        var intervals = GetLastMinuteLogsInterval();

        return new DailyStatistics()
        {
            LogsCount = logsCount,
            TotalCount = totalCount,
            LogsInterval = intervals
        };
    }

    private (long totalCount, List<LogsCount> logsCount) GetDailyLogCount()
    {
        var totalCount = 0;
        var logsCount = new List<LogsCount>();

        var currentDayStart = DateTime.UtcNow.Date;
        var currentDayEnds = DateTime.UtcNow.Date.AddDays(1);

        foreach (var logLevel in Enum.GetValues<LogLevel>())
        {
            var count = _Collection
                .AsQueryable()
                .Count(entity =>
                    entity.Level == logLevel &&
                    entity.Timestamp >= currentDayStart &&
                    entity.Timestamp < currentDayEnds);

            totalCount += count;
            logsCount.Add(new LogsCount(logLevel, count));
        }

        return (totalCount, logsCount);
    }

    private Dictionary<DateTime, int> GetLastMinuteLogsInterval()
    {
        var lastMinuteEnd = DateTime.UtcNow;
        var lastMinuteStart = lastMinuteEnd.AddMinutes(-1);

        var lastMinuteLogs = _Collection
                .AsQueryable()
                .Where(entity =>
                    entity.Timestamp >= lastMinuteStart &&
                    entity.Timestamp < lastMinuteEnd)
                .ToList();

        int secondsInInterval = 10;
        Dictionary<DateTime, int> intervals = new();

        for (var intervalStart = lastMinuteStart; intervalStart < lastMinuteEnd; intervalStart = intervalStart.AddSeconds(secondsInInterval))
        {
            var logsInInterval = lastMinuteLogs.Count(entity =>
                    entity.Timestamp >= intervalStart &&
                    entity.Timestamp < intervalStart.AddSeconds(secondsInInterval));
            intervals.Add(intervalStart, logsInInterval / secondsInInterval);
        }

        return intervals;
    }

    public LogsStatistics GetTotalStatistics()
    {
        long totalCount = 0;
        var logsCount = new List<LogsCount>();
        foreach (var logLevel in Enum.GetValues<LogLevel>())
        {
            var count = _Collection
                .AsQueryable()
                .Count(entity => entity.Level == logLevel);
            totalCount += count;
            logsCount.Add(new LogsCount(logLevel, count));
        }

        return new LogsStatistics()
        {
            LogsCount = logsCount,
            TotalCount = totalCount
        };
    }
}
