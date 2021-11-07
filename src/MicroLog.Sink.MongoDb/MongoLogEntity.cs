using MicroLog.Core;
using MicroLog.Core.Abstractions;
using MicroLog.Sink.MongoDb.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Sink.MongoDb
{
    internal class MongoLogEntity : ILogEvent
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
                        string json;
                        // Before performing save to database _properties dictionary
                        // stores BsonDocument objects, but when MongoLogEntity object
                        // is recraeted from database the type of values in dictionary
                        // is KeyValuePair<>
                        if (property.Value is not BsonDocument bson)
                        {
                            // Create BsonDocument from KeyValuePair<> object
                            bson = property.Value.ToBsonDocument();
                            // BsonDocument contains the type as first value,
                            // the second value stores saved object in json
                            json = bson.ElementAt(1).Value.ToJson();
                        }
                        else
                        {
                            json = bson.ToJson();
                        }
                        return new LogProperty { Name = property.Key, Value = json };
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
                _properties.Add(prop.Name, prop.BsonValue);
            }
        }

        public override bool Equals(object obj)
        {
            return obj is ILogEvent entity &&
                   Identity.Equals(entity.Identity) &&
                   Message == entity.Message &&
                   new DateComparer().Equals(Timestamp, entity.Timestamp) &&
                   Level == entity.Level &&
                   Properties.SequenceEqual(entity.Properties);
                   /*Exception.GetType().Equals(entity.Exception.GetType()) &&
                   Exception.Message.Equals(entity.Exception.Message);*/
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Identity, Message, Timestamp, Level, Exception, Properties);
        }
    }
}
