using MicroLog.Sink.MongoDb.Utils;
using MongoDB.Bson.Serialization.Serializers;

namespace MicroLog.Sink.MongoDb;

/// <summary>
/// A MongoDb log event representation.
/// </summary>
internal class MongoLogEntity : ILogEvent
{
    [BsonId]
    [BsonSerializer(typeof(ImpliedImplementationInterfaceSerializer<ILogEventIdentity, MongoLogIdentity>))]
    public ILogEventIdentity Identity { get; init; }
    public string Message { get; set; }
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime Timestamp { get; init; }
    public LogLevel Level { get; set; }
    public LogException Exception { get; set; }

    [BsonExtraElements]
    private BsonDocument _properties { get; set; } = new();

    [BsonIgnore]
    public IEnumerable<ILogProperty> Properties
    {
        get
        {
            return _properties.Select(property =>
            {
                return new LogProperty {
                    Name = property.Name,
                    Value = property.Value.ToBsonDocument().ToJson()
                };
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
