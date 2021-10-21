using MicroLog.Core;
using System;
using Xunit;
using Shouldly;
using MicroLog.Driver.MongoDb.Tests.Fixture;

namespace MicroLog.Driver.MongoDb.Tests
{
    public class LogRepositoryCases : LogRepositoryFixture
    {
        [Fact]
        public void Log_Save_Success()
        {
            using var db = Container().Start();
            var connectionString = GetConnectionString(db);

            var logCollector = CreateMongoLogCollector(connectionString);
            var logRegistry = CreateMongoLogRegistry(connectionString);

            var logEvent = new MongoLogEntity()
            {
                Level = LogLevel.Information,
                Message = "Works !!!"
            };

            logCollector.InsertAsync(logEvent).Wait();
            var log = logRegistry.GetAsync(logEvent.Identity).GetAwaiter().GetResult();

            log.ShouldBe(logEvent);
        }
    }
}
