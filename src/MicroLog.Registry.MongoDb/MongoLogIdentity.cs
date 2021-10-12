using MicroLog.Core;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Registry.MongoDb
{
    public class MongoLogIdentity : ILogEventIdentity
    {
        public string EventId { get; init; }
        public string ServiceId { get; init; }

        public MongoLogIdentity()
        {
            EventId = Guid.NewGuid().ToString();
            ServiceId = string.Empty;
        }

        public MongoLogIdentity(string eventId, string serviceId)
        {
            EventId = eventId;
            ServiceId = serviceId;
        }

        public override bool Equals(object obj)
        {
            return obj is MongoLogIdentity identity &&
                   EventId == identity.EventId &&
                   ServiceId == identity.ServiceId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(EventId, ServiceId);
        }
    }
}
