﻿using MicroLog.Core.Abstractions;
using MicroLog.Sink.MongoDb.Config;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Sink.MongoDb
{
    public class MongoLogRepository : ILogSink, ILogRegistry
    {
        private const string COLLECTION_NAME = "logs";
        private MongoConfig _Config { get; }
        private IMongoDatabase _Database { get; }
        private IMongoCollection<MongoLogEntity> _Collection { get; }

        public MongoLogRepository(IOptions<MongoConfig> configOptions)
            : this(configOptions.Value)
        {
        }

        public MongoLogRepository(MongoConfig config)
        {
            _Config = config;
            var client = new MongoClient(config.ConnectionString);
            _Database = client.GetDatabase(config.DatabaseName);
            _Collection = _Database.GetCollection<MongoLogEntity>(COLLECTION_NAME);
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

        async Task ILogSink.InsertAsync(ILogEvent logEntity)
        {
            var entity = MongoLogMapper.Map(logEntity);
            await _Collection.InsertOneAsync(entity);
        }

        async Task ILogSink.InsertAsync(IEnumerable<ILogEvent> logEntities)
        {
            var entities = logEntities.Select(entity => MongoLogMapper.Map(entity));
            await _Collection.InsertManyAsync(entities);
        }

        public ISinkConfig GetConfiguration() => _Config;
    }
}
