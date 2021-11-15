using MicroLog.Core;
using MicroLog.Core.Enrichers;
using MicroLog.IntegrationTests.MongoDb.Fixture;
using MicroLog.Sink.MongoDb;

namespace MicroLog.IntegrationTests.MongoDb.Cases;

public class MongoLogRepositoryCases : MongoFixture
{
    [Fact]
    public void Log_Save_Success()
    {
        using var db = Container().Start();
        var connectionString = GetConnectionString(db);

        var logSink = CreateMongoLogSink(connectionString);
        var logRegistry = CreateMongoLogRegistry(connectionString);

        var logEvent = new MongoLogEntity()
        {
            Level = LogLevel.Information,
            Message = "Works !!!"
        };

        logSink.InsertAsync(logEvent).GetAwaiter().GetResult();
        var log = logRegistry.GetAsync(logEvent.Identity).GetAwaiter().GetResult();

        log.ShouldBe(logEvent);
    }

    [Fact]
    public void Log_Enrich_Success()
    {
        using var db = Container().Start();
        var connectionString = GetConnectionString(db);

        var logSink = CreateMongoLogSink(connectionString);
        var logRegistry = CreateMongoLogRegistry(connectionString);

        var logEvent = new LogEvent()
        {
            Level = LogLevel.Information,
            Message = "Works !!!"
        };

        var enricher = new ValueEnricher("Test", "Value");
        enricher.Enrich(logEvent);

        logSink.InsertAsync(logEvent).GetAwaiter().GetResult();
        var log = logRegistry.GetAsync(logEvent.Identity).GetAwaiter().GetResult();

        var mongoLogEvent = MongoLogMapper.Map(logEvent);
        log.ShouldBe(mongoLogEvent);
    }
}
