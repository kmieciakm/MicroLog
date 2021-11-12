namespace MicroLog.Collector.Config;

public class SinksConfig
{
    public IEnumerable<MongoConfig> Mongo { get; set; }
    public IEnumerable<HubConfig> Hub { get; set; }
}

public class PublisherConfig : IPublisherConfig
{
    public string Queues { get; set; } = string.Empty;

    public bool IsPublisherSpecified
    {
        get
        {
            return !string.IsNullOrEmpty(Queues);
        }
    }

    public IEnumerable<string> GetQueues()
    {
        return Queues.Split(',');
    }
}
