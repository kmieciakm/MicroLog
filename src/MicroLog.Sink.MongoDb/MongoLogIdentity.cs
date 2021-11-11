using MicroLog.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Sink.MongoDb
{
    /// <summary>
    /// A MongoDb log identity.
    /// </summary>
    internal class MongoLogIdentity : ILogEventIdentity
    {
        public string EventId { get; init; }

        public MongoLogIdentity()
        {
            EventId = Guid.NewGuid().ToString();
        }

        public MongoLogIdentity(string eventId)
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
