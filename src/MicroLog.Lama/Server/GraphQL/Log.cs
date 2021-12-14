namespace MircoLog.Lama.Server.GraphQL;

public class Log
{
    [BsonId]
    [BsonSerializer(typeof(ImpliedImplementationInterfaceSerializer<ILogEventIdentity, LogIdentity>))]
    [GraphQLType(typeof(LogIdentity))]
    public ILogEventIdentity Identity { get; init; }
    public string Message { get; set; }
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime Timestamp { get; set; }
    public MicroLog.Core.LogLevel Level { get; set; }
    public string LevelName => Enum.GetName(typeof(MicroLog.Core.LogLevel), Level);
    public LogException Exception { get; set; }

    [BsonExtraElements]
    private BsonDocument _properties { get; set; } = new();

    [BsonIgnore]
    public IEnumerable<LogProperty> Properties
    {
        get
        {
            return _properties.Select(property =>
            {
                return new LogProperty
                {
                    Name = property.Name,
                    Value = property.Value.ToBsonDocument().ToJson()
                };
            });
        }
        set
        {
            foreach (var prop in value)
            {
                _properties.Add(prop.Name, BsonDocument.Parse(prop.Value));
            };
        }
    }

    public static IEnumerable<Log> Parse(IEnumerable<ILogEvent> logs)
    {
        return logs.Select(log => new Log()
        {
            Identity = new LogIdentity()
            {
                EventId = log.Identity.EventId
            },
            Message = log.Message,
            Timestamp = log.Timestamp,
            Level = log.Level,
            Exception = log.Exception,
            Properties = log.Properties
            .Select(prop =>
                new LogProperty()
                {
                    Name = prop.Name,
                    Value = prop.Value,
                }
            )
        });
    }
}