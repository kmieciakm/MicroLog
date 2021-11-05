using MicroLog.Core;
using MicroLog.Core.Abstractions;
using MicroLog.Sink.MongoDb.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Sink.MongoDb
{
    class MongoLogEntity : ILogEvent
    {
        [BsonId]
        public ILogEventIdentity Identity { get; private set; }
        public string Message { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Timestamp { get; init; }
        public LogLevel Level { get; set; }
        public Exception Exception { get; set; }

        [BsonExtraElements]
        private Dictionary<string, object> _properties { get; set; } = new();
        [BsonIgnore]
        public IReadOnlyCollection<ILogProperty> Properties
            => _properties
                .Select(property =>
                    {
                        var bsonValues = property.Value.ToBsonDocument().Values;
                        // skip first value that specifies the type and get second that stores saved object in json
                        var json = bsonValues.ElementAt(1).ToJson();
                        return new MongoLogProperty(property.Key, json);
                    })
                .ToList()
                .AsReadOnly();

        public MongoLogEntity()
        {
            Identity = new MongoLogIdentity();
            Timestamp = DateTime.Now;
        }

        public MongoLogEntity(MongoLogIdentity identity, string message, DateTime timestamp, LogLevel level, Exception exception, IEnumerable<MongoLogProperty> properties)
        {
            Identity = identity;
            Message = message;
            Timestamp = timestamp;
            Level = level;
            Exception = exception;
            foreach (var prop in properties)
            {
                BsonDocument doc = BsonDocument.Parse(prop.Value);
                _properties.Add(prop.Name, doc);
            }
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
