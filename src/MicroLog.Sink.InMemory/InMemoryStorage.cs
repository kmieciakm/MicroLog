using MicroLog.Core;
using MicroLog.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MicroLog.Sink.InMemory;

/// <summary>
/// Dummy configuration for <see cref="InMemoryStorage"/>.
/// </summary>
public class InMemoryConfig : ISinkConfig
{
    public string Name { get; set; } = "InMemory";
}

/// <summary>
/// In-memory storage.
/// </summary>
public class InMemoryStorage : ILogSink, ILogRegistry
{
    public ISinkConfig Config { get; } = new InMemoryConfig();

    private Dictionary<string, ILogEvent> Storage { get; set; }

    public Task InsertAsync(ILogEvent logEvent)
    {
        Storage.Add(logEvent.Identity.EventId, logEvent);
        return Task.CompletedTask;
    }

    public Task InsertAsync(IEnumerable<ILogEvent> logEvents)
    {
        foreach(var logEntity in logEvents)
        {
            Storage.Add(logEntity.Identity.EventId, logEntity);
        }
        return Task.CompletedTask;
    }

    Task<PaginationResult<ILogEvent>> ILogRegistry.GetAsync(int skip, int take)
    {
        IEnumerable<ILogEvent> logs = Storage.Values
            .Skip(skip)
            .Take(take)
            .ToList();

        var result = new PaginationResult<ILogEvent>()
        {
            Items = logs,
            TotalCount = logs.Count()
        };

        return Task.FromResult(result);
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
