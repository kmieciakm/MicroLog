using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Core.Abstractions
{
    /// <summary>
    /// Log message.
    /// </summary>
    public interface ILogEvent
    {
        ILogEventIdentity Identity { get; }
        string Message { get; set; }
        DateTime Timestamp { get; init; }
        LogLevel Level { get; set; }
        Exception Exception { get; set; }
    }
}
