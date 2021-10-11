using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Core
{
    public interface ILogEvent
    {
        ILogEventIdentity Identity { get; set; }
        string Message { get; set; }
        DateTime Timestamp { get; set; }
        LogLevel Level { get; set; }
        Exception Exception { get; set; }
    }
}
