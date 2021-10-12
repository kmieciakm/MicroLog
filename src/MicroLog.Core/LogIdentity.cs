using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Core
{
    class LogIdentity : ILogEventIdentity
    {
        public string EventId { get; init; }
        public string ServiceId { get; init; }

        public LogIdentity(string serviceId)
        {
            EventId = Guid.NewGuid().ToString();
            ServiceId = serviceId;
        }

        public LogIdentity(string eventId, string serviceId)
        {
            EventId = eventId;
            ServiceId = serviceId;
        }

        public override bool Equals(object obj)
        {
            return obj is LogIdentity identity &&
                   EventId == identity.EventId &&
                   ServiceId == identity.ServiceId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(EventId, ServiceId);
        }
    }
}
