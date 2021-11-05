using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Core.Abstractions
{
    /// <summary>
    /// Send log entries to the Message Queue.
    /// </summary>
    public interface ILogPublisher
    {
        Task PublishAsync(ILogEvent logEntity);
        Task PublishAsync(IEnumerable<ILogEvent> logEntities);
    }
}
