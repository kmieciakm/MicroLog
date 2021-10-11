using MicroLog.Core;
using MicroLog.Core.Infrastructure;
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
        Task<ILogEvent> ILogRegistry.GetAsync(ILogEventIdentity id)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<ILogEvent>> ILogRegistry.GetAsync(IEnumerable<ILogEventIdentity> ids)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<ILogEvent>> ILogRegistry.GetAsync(Expression<Func<ILogEvent, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        Task ILogSink.InsertAsync(ILogEvent logEntity)
        {
            throw new NotImplementedException();
        }

        Task ILogSink.InsertAsync(IEnumerable<ILogEvent> logEntities)
        {
            throw new NotImplementedException();
        }
    }
}
