namespace MircoLog.Lama.Server.GraphQL;

public class Query
{
    public async Task<IEnumerable<Log>> GetLogs([Service] ILogRegistry registry)
    {
        var logs = await registry.GetAsync();
        return Log.Parse(logs);
    }
}
