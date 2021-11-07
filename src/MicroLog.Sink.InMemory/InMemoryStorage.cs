using MicroLog.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MicroLog.Sink.InMemory
{
    public class InMemoryConfig : ISinkConfig
    {
        public string Name { get; set; } = "InMemory";
    }

    public class InMemoryStorage : ILogSink, ILogRegistry
    {
        private Dictionary<string, ILogEvent> Storage { get; set; }

        public ISinkConfig GetConfiguration() => new InMemoryConfig();

        public Task InsertAsync(ILogEvent logEntity)
        {
            Storage.Add(logEntity.Identity.EventId, logEntity);
            return Task.CompletedTask;
        }

        public Task InsertAsync(IEnumerable<ILogEvent> logEntities)
        {
            foreach(var logEntity in logEntities)
            {
                Storage.Add(logEntity.Identity.EventId, logEntity);
            }
            return Task.CompletedTask;
        }

        Task<ILogEvent> ILogRegistry.GetAsync(ILogEventIdentity identity)
        {
            return Task.FromResult(Storage[identity.EventId]);
        }

        Task<IEnumerable<ILogEvent>> ILogRegistry.GetAsync(IEnumerable<ILogEventIdentity> identities)
        {
            var logs = Storage.Values.Where(log => identities.Any(id => log.Identity.Equals(id)));
            return Task.FromResult(logs);
        }

        Task<IEnumerable<ILogEvent>> ILogRegistry.GetAsync(Expression<Func<ILogEvent, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
