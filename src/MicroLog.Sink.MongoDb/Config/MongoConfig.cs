namespace MicroLog.Sink.MongoDb.Config;

/// <summary>
/// Configuration of the MongoDb connection.
/// </summary>
public class MongoConfig : ISinkConfig
{
    public string Name { get; set; }
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}
