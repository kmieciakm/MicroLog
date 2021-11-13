namespace MicroLog.Collector.Config;

public class SinksConfig
{
    public IEnumerable<MongoConfig> Mongo { get; set; }
    public IEnumerable<HubConfig> Hub { get; set; }
}
