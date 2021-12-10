namespace MircoLog.Lama.Server.GraphQL;

public class Log
{
    public LogIdentity Identity { get; init; }
    public string Message { get; set; }
    public DateTime Timespan { get; set; }
    public MicroLog.Core.LogLevel Level { get; set; }
    public string LevelName { get; set; }
    public LogException Exception { get; set; }
    public IEnumerable<LogProperty> Properties { get; set; }

    public static IEnumerable<Log> Parse(IEnumerable<ILogEvent> logs)
    {
        return logs.Select(log => new Log()
        {
            Identity = LogIdentity.Parse(log.Identity),
            Message = log.Message,
            Timespan = log.Timestamp,
            Level = log.Level,
            LevelName = Enum.GetName(typeof(MicroLog.Core.LogLevel), log.Level),
            Exception = log.Exception,
            Properties = log.Properties.Select(prop =>
                new LogProperty()
                {
                    Name = prop.Name,
                    Value = prop.Value,
                }
            )
        });
    }
}
