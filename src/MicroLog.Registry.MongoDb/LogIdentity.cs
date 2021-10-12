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
    public class LogIdentity : ILogEventIdentity
    {
        public string Id { get; set; }

        public LogIdentity()
        {
            Id = ObjectId.GenerateNewId().ToString();
        }

        public LogIdentity(string id)
        {
            Id = id;
        }

        public LogIdentity(ObjectId id)
        {
            Id = id.ToString();
        }

        public override bool Equals(object obj)
        {
            return obj is LogIdentity identity &&
                   Id == identity.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
