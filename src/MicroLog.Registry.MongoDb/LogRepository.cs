using MicroLog.Core;
using MicroLog.Core.Infrastructure;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Registry.MongoDb
{
    public class LogRepository : ILogSink, ILogRegistry
    {
        private IMongoCollection<LogEntity> _Collection { get; }

        public LogRepository(IMongoCollection<LogEntity> collection)
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
            Func<LogEntity, bool> func = predicate.Compile();
            using var entities = await _Collection.FindAsync(entity => func.Invoke(entity));
            return entities.ToEnumerable();
        }

        async Task ILogSink.InsertAsync(ILogEvent logEntity)
        {
            var entity = logEntity as LogEntity;
            await _Collection.InsertOneAsync(entity);
        }

        async Task ILogSink.InsertAsync(IEnumerable<ILogEvent> logEntities)
        {
            var entities = logEntities as IEnumerable<LogEntity>;
            await _Collection.InsertManyAsync(entities);
        }
    }
}
