namespace MircoLog.Lama.Server.GraphQL;

public class Query
{
    public async Task<IEnumerable<Log>> GetLogs([Service] ILogRegistry registry)
    {
        var logs = await registry.GetAsync();
        return Log.Parse(logs);
    }
}

public class Log
{
    public string Message { get; set; }
    public DateTime Timespan { get; set; }

    public static IEnumerable<Log> Parse(IEnumerable<ILogEvent> logs)
    {
        return logs.Select(log => new Log()
        {
            Message = log.Message,
            Timespan = log.Timestamp
        });
    }
}