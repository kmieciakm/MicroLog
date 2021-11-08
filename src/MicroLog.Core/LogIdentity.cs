using MicroLog.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Core
{
    /// <summary>
    /// Base <see cref="ILogEventIdentity"/> implementation.
    /// </summary>
    public class LogIdentity : ILogEventIdentity
    {
        public string EventId { get; init; }

        public LogIdentity()
        {
            EventId = Guid.NewGuid().ToString();
        }

        public LogIdentity(string eventId)
        {
            EventId = eventId;
        }

        public override bool Equals(object obj)
        {
            return obj is ILogEventIdentity identity &&
                   EventId == identity.EventId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(EventId);
        }
    }
}
