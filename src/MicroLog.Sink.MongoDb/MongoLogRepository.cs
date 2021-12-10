using MicroLog.Sink.MongoDb.Config;
using Microsoft.Extensions.Options;
using Polly;

namespace MicroLog.Sink.MongoDb;

/// <summary>
/// A MongoDb data access point of logs.
/// </summary>
public class MongoLogRepository : ILogSink, ILogRegistry
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

    async Task<IEnumerable<ILogEvent>> ILogRegistry.GetAsync()
    {
        var entities = await _Collection.FindAsync(_ => true);
        return entities.ToEnumerable();
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

    async Task<IEnumerable<ILogEvent>> ILogRegistry.GetAsync(Expression<Func<ILogEvent, bool>> predicate)
    {
        Func<MongoLogEntity, bool> func = predicate.Compile();
        using var entities = await _Collection.FindAsync(entity => func.Invoke(entity));
        return entities.ToEnumerable();
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

    /// <summary>
    /// Insert retry policy.
    /// </summary>
    private IAsyncPolicy GetInsertPolicy()
    {
        var maxNumberOfRetry = 10;
        var pauseBetweenFailures = TimeSpan.FromSeconds(1);
        var retryPolicy = Policy
             .Handle<Exception>()
             .WaitAndRetryAsync(maxNumberOfRetry, _ => pauseBetweenFailures);
        return retryPolicy;
    }
}
