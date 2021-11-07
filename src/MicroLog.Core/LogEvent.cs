using MicroLog.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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

        private Dictionary<string, ILogProperty> _properties { get; set; } = new();
        public IReadOnlyCollection<ILogProperty> Properties => _properties.Values.ToList().AsReadOnly();

        public LogEvent()
        {
            Identity = new LogIdentity();
            Timestamp = DateTime.Now;
        }

        public LogEvent(ILogEventIdentity identity, string message, DateTime timestamp, LogLevel level, Exception exception, IEnumerable<LogProperty> properties)
        {
            Identity = identity;
            Message = message;
            Timestamp = timestamp;
            Level = level;
            Exception = exception;
            foreach (var prop in properties)
            {
                _properties.Add(prop.Name, prop);
            }
        }

        public void Enrich(ILogProperty logProperty)
        {
            if (!_properties.ContainsKey(logProperty.Name))
            {
                _properties.Add(logProperty.Name, logProperty);
            }
        }

        public override bool Equals(object obj)
        {
            return obj is ILogEvent @event &&
                   Identity.Equals(@event.Identity) &&
                   Message == @event.Message &&
                   Timestamp == @event.Timestamp &&
                   Level == @event.Level &&
                   Properties.SequenceEqual(@event.Properties);
                   /*Exception.GetType().Equals(@event.Exception.GetType()) &&
                   Exception.Message.Equals(@event.Exception.Message);*/
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Identity, Message, Timestamp, Level, Exception, Properties);
        }
    }
}
