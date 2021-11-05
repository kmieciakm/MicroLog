using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Core.Abstractions
{
    /// <summary>
    /// Writes log entries to the permanent data storage.
    /// </summary>
    public interface ILogSink
    {
        ISinkConfig GetConfiguration();
        Task InsertAsync(ILogEvent logEntity);
        Task InsertAsync(IEnumerable<ILogEvent> logEntities);
    }
}
