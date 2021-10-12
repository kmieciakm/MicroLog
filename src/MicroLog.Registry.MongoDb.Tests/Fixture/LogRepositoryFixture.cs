using MicroLog.Core.Infrastructure;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Registry.MongoDb.Tests.Fixture
{
    public abstract class LogRepositoryFixture : MongoIntegrationFixture
    {
        private static IMongoCollection<MongoLogEntity> CreateMongoTestCollection(string connectionString)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("TestDatabase");
            var collection = database.GetCollection<MongoLogEntity>("TestCollection");
            return collection;
        }

        protected static ILogSink CreateMongoLogSink(string connectionString)
        {
            var collection = CreateMongoTestCollection(connectionString);
            return new MongoLogRepository(collection);
        }

        protected static ILogRegistry CreateMongoLogRegistry(string connectionString)
        {
            var collection = CreateMongoTestCollection(connectionString);
            return new MongoLogRepository(collection);
        }
    }
}
