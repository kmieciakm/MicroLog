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

    public LogsStatistics GetDailyStatistics()
    {
        long totalCount = 0;
        var logsCount = new List<LogsCount>();

        DateTime currentDayStart = DateTime.Now.Date;
        DateTime currentDayEnds = DateTime.Now.Date.AddDays(1);

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

        return new LogsStatistics()
        {
            LogsCount = logsCount,
            TotalCount = totalCount
        };
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
