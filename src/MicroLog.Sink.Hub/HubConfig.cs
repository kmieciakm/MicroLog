namespace MicroLog.Sink.Hub;

public class HubConfig : ISinkConfig
{
    public string Name { get; set; }
    public string Url { get; set; }
}
