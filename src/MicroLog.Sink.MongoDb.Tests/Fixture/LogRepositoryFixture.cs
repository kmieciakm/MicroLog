using MicroLog.Core.Abstractions;
using MicroLog.Sink.MongoDb;
using MicroLog.Sink.MongoDb.Config;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Sink.MongoDb.Tests.Fixture
{
    public abstract class LogRepositoryFixture : MongoIntegrationFixture
    {
        private static MongoSinkConfig CreateMongoTestConfig(string connectionString) 
            => new MongoSinkConfig()
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
