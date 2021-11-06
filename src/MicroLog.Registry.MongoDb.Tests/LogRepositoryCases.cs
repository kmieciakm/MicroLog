using MicroLog.Core;
using System;
using Xunit;
using Shouldly;
using MicroLog.Sink.MongoDb.Tests.Fixture;
using MicroLog.Core.Enrichers;

namespace MicroLog.Sink.MongoDb.Tests
{
    public class LogRepositoryCases : LogRepositoryFixture
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

            logSink.InsertAsync(logEvent).Wait();
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

            logSink.InsertAsync(logEvent).Wait();
            var log = logRegistry.GetAsync(logEvent.Identity).GetAwaiter().GetResult();

            var mongoLogEvent = MongoLogMapper.Map(logEvent);
            log.ShouldBe(mongoLogEvent);
        }
    }
}
