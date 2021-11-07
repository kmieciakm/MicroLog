using MicroLog.Core.Abstractions;
using MicroLog.Sink.MongoDb;
using MicroLog.Sink.MongoDb.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.IntegrationTests.MongoDb.Fixture
{
    public abstract class MongoFixture : MongoIntegrationFixture
    {
        private static MongoConfig CreateMongoTestConfig(string connectionString)
            => new MongoConfig()
            {
                ConnectionString = connectionString,
                DatabaseName = "TestDatabase"
            };

        protected static ILogSink CreateMongoLogSink(string connectionString)
        {
            var config = CreateMongoTestConfig(connectionString);
            return new MongoLogRepository(config);
        }

        protected static ILogRegistry CreateMongoLogRegistry(string connectionString)
        {
            var config = CreateMongoTestConfig(connectionString);
            return new MongoLogRepository(config);
        }
    }
}
