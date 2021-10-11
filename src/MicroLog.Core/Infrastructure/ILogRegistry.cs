using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Core.Infrastructure
{
    public interface ILogRegistry
    {
        Task<ILogEvent> GetAsync(ILogEventIdentity id);
        Task<IEnumerable<ILogEvent>> GetAsync(IEnumerable<ILogEventIdentity> ids);
        Task<IEnumerable<ILogEvent>> GetAsync(Expression<Func<ILogEvent, bool>> predicate);
    }
}
