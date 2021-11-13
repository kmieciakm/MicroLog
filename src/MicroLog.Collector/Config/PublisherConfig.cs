namespace MicroLog.Collector.Config;

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
