using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Core.Abstractions
{
    /// <summary>
    /// Provides access to logs persisted in a data storage.
    /// </summary>
    public interface ILogRegistry
    {
        Task<ILogEvent> GetAsync(ILogEventIdentity identity);
        Task<IEnumerable<ILogEvent>> GetAsync(IEnumerable<ILogEventIdentity> identities);
        Task<IEnumerable<ILogEvent>> GetAsync(Expression<Func<ILogEvent, bool>> predicate);
    }
}
