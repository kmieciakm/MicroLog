using MicroLog.Core;
using MicroLog.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Registry.MongoDb
{
    public class LogEntity : ILogEvent
    {
        public ILogEventIdentity Identity { get; set; } = new LogIdentity();
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public LogLevel Level { get; set; }
        public Exception Exception { get; set; }
    }
}
