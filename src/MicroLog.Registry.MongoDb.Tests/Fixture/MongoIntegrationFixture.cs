using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using Ductus.FluentDocker.Services.Extensions;
using Ductus.FluentDocker.Services.Impl;
using MongoDB.Bson;
using MongoDB.Driver;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MicroLog.Sink.MongoDb.Tests.Fixture
{
    public abstract class MongoIntegrationFixture
    {
        const string root = "root";
        const string secret = "secret";

        protected static IContainerService Container()
        {
            return (DockerContainerService)new Builder()
                .UseContainer()
                .UseImage("mongo")
                .ExposePort(27017)
                .WithEnvironment(new[] {
                    $"MONGO_INITDB_ROOT_USERNAME={root}",
                    $"MONGO_INITDB_ROOT_PASSWORD={secret}"})
                .WaitForPort("27017/tcp", 30000)
                .Build();
        }

        protected static string GetConnectionString(IContainerService container)
        {
            var port = container.ToHostExposedEndpoint("27017/tcp");
            var connectionString = $"mongodb://{root}:{secret}@{port}";
            return connectionString;
        }

        [Fact]
        public void Mongo_Running()
        {
            using var db = Container().Start();
            Assert.Equal(ServiceRunningState.Running, db.State);
        }

        [Fact]
        public void Mongo_Connected()
        {
            using var db = Container().Start();

            var client = new MongoClient(GetConnectionString(db));
            var database = client.GetDatabase("Test");

            bool isMongoLive = database
                .RunCommandAsync((Command<BsonDocument>)"{ping:1}")
                .Wait(5000);

            isMongoLive.ShouldBeTrue();
        }
    }
}
