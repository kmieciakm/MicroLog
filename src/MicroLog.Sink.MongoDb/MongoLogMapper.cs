namespace MicroLog.Sink.MongoDb;

internal static class MongoLogMapper
{
    public static MongoLogEntity Map(ILogEvent log)
        => new()
        {
            Identity = new MongoLogIdentity(log.Identity.EventId),
            Message = log.Message,
            Timestamp = log.Timestamp,
            Level = log.Level,
            Exception = log.Exception,
            Properties = log.Properties.Select(prop => new MongoLogProperty(prop))
        };

    public static IEnumerable<MongoLogProperty> MapProperties(IEnumerable<ILogProperty> properties)
    {
        List<MongoLogProperty> mongoLogProperties = new();
        foreach (var property in properties)
        {
            mongoLogProperties.Add(new MongoLogProperty(property));
        }
        return mongoLogProperties;
    }
}
