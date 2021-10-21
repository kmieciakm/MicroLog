using MicroLog.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Core
{
    public class LogEvent : ILogEvent
    {
        public ILogEventIdentity Identity { get; }
        public string Message { get; set; }
        public DateTime Timestamp { get; init; }
        public LogLevel Level { get; set; }
        public Exception Exception { get; set; }

        public LogEvent()
        {
            Identity = new LogIdentity();
            Timestamp = DateTime.Now;
        }

        public LogEvent(ILogEventIdentity identity, string message, DateTime timestamp, LogLevel level, Exception exception)
        {
            Identity = identity;
            Message = message;
            Timestamp = timestamp;
            Level = level;
            Exception = exception;
        }

        public override bool Equals(object obj)
        {
            return obj is LogEvent @event &&
                   Identity.Equals(@event.Identity) &&
                   Message == @event.Message &&
                   Timestamp == @event.Timestamp &&
                   Level == @event.Level &&
                   EqualityComparer<Exception>.Default.Equals(Exception, @event.Exception);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Identity, Message, Timestamp, Level, Exception);
        }
    }
}
