using MicroLog.Core;
using MicroLog.Core.Abstractions;
using MicroLog.Driver.MongoDb.Utils;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Driver.MongoDb
{
    public class MongoLogEntity : ILogEvent
    {
        [BsonId]
        public ILogEventIdentity Identity { get; private set; }
        public string Message { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Timestamp { get; init; }
        public LogLevel Level { get; set; }
        public Exception Exception { get; set; }

        public MongoLogEntity()
        {
            Identity = new MongoLogIdentity();
            Timestamp = DateTime.Now;
        }

        public MongoLogEntity(MongoLogIdentity identity, string message, DateTime timestamp, LogLevel level, Exception exception)
        {
            Identity = identity;
            Message = message;
            Timestamp = timestamp;
            Level = level;
            Exception = exception;
        }

        public override bool Equals(object obj)
        {
            return obj is MongoLogEntity entity &&
                   Identity.Equals(entity.Identity) &&
                   Message == entity.Message &&
                   new DateComparer().Equals(Timestamp, entity.Timestamp) &&
                   Level == entity.Level &&
                   EqualityComparer<Exception>.Default.Equals(Exception, entity.Exception);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Identity, Message, Timestamp, Level, Exception);
        }
    }
}
