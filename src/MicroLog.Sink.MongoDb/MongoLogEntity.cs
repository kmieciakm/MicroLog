using MicroLog.Sink.MongoDb.Utils;

namespace MicroLog.Sink.MongoDb;

/// <summary>
/// A MongoDb log event representation.
/// </summary>
internal class MongoLogEntity : ILogEvent
{
    [BsonId]
    public ILogEventIdentity Identity { get; init; }
    public string Message { get; set; }
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime Timestamp { get; init; }
    public LogLevel Level { get; set; }
    public LogException Exception { get; set; }

    [BsonExtraElements]
    private Dictionary<string, object> _properties { get; set; } = new();

    [BsonIgnore]
    public IEnumerable<ILogProperty> Properties
    {
        get
        {
            return _properties.Select(property =>
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
                });
        }
        init
        {
            foreach (var prop in MongoLogMapper.MapProperties(value))
            {
                _properties.Add(prop.Name, prop.BsonValue);
            };
        }
    }

    public MongoLogEntity()
    {
        Identity = new MongoLogIdentity();
        Timestamp = DateTime.Now;
    }

    public MongoLogEntity(MongoLogIdentity identity, string message, DateTime timestamp, LogLevel level, LogException exception, IEnumerable<MongoLogProperty> properties)
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
               Properties.SequenceEqual(entity.Properties) &&
               Exception is not null ? Exception.Equals(entity.Exception) : true;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Identity, Message, Timestamp, Level, Exception, Properties);
    }
}
