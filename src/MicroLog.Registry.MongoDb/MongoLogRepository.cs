using MicroLog.Core.Abstractions;
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
        private IMongoCollection<MongoLogEntity> _Collection { get; }

        public MongoLogRepository(IMongoCollection<MongoLogEntity> collection)
        {
            _Collection = collection;
        }

        async Task<ILogEvent> ILogRegistry.GetAsync(ILogEventIdentity identity)
        {
            using var entity = await _Collection.FindAsync(entity => entity.Identity == identity);
            return entity.FirstOrDefault();
        }

        async Task<IEnumerable<ILogEvent>> ILogRegistry.GetAsync(IEnumerable<ILogEventIdentity> identities)
        {
            using var entity = await _Collection.FindAsync(entity => entity.Identity == identities);
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
            var entity = logEntity as MongoLogEntity;
            await _Collection.InsertOneAsync(entity);
        }

        async Task ILogSink.InsertAsync(IEnumerable<ILogEvent> logEntities)
        {
            var entities = logEntities as IEnumerable<MongoLogEntity>;
            await _Collection.InsertManyAsync(entities);
        }
    }
}
