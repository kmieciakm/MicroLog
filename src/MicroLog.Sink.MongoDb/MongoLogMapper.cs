namespace MicroLog.Sink.MongoDb;

internal static class MongoLogMapper
{
    public static MongoLogEntity Map(ILogEvent log) => new MongoLogEntity(
            identity: new MongoLogIdentity(log.Identity.EventId),
            message: log.Message,
            timestamp: log.Timestamp,
            level: log.Level,
            exception: log.Exception,
            properties: log.Properties.Select(prop => new MongoLogProperty(prop))
        );

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
