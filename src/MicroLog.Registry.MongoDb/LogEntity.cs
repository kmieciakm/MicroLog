using MicroLog.Core;
using MicroLog.Core.Infrastructure;
using MicroLog.Registry.MongoDb.Utils;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Registry.MongoDb
{
    public class LogEntity : ILogEvent
    {
        [BsonId]
        public ILogEventIdentity Identity { get; set; }
        public string Message { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Timestamp { get; set; }
        public LogLevel Level { get; set; }
        public Exception Exception { get; set; }

        public LogEntity()
        {
            Identity = new LogIdentity();
        }

        public LogEntity(LogIdentity identity, string message, DateTime timestamp, LogLevel level, Exception exception)
        {
            Identity = identity;
            Message = message;
            Timestamp = timestamp;
            Level = level;
            Exception = exception;
        }

        public override bool Equals(object obj)
        {
            return obj is LogEntity entity &&
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
