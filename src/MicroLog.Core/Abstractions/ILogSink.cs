using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Core.Abstractions
{
    public interface ILogSink
    {
        Task InsertAsync(ILogEvent logEntity);
        Task InsertAsync(IEnumerable<ILogEvent> logEntities);
    }
}
