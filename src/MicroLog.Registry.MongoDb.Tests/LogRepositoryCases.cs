using MicroLog.Core;
using System;
using Xunit;
using Shouldly;
using MicroLog.Registry.MongoDb.Tests.Fixture;

namespace MicroLog.Registry.MongoDb.Tests
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

            var logEvent = new LogEntity()
            {
                Identity = new LogIdentity(),
                Level = LogLevel.Information,
                Message = "Works !!!",
                Timestamp = DateTime.Now
            };

            logSink.InsertAsync(logEvent).Wait();
            var log = logRegistry.GetAsync(logEvent.Identity).GetAwaiter().GetResult();

            log.ShouldBe(logEvent);
        }
    }
}
