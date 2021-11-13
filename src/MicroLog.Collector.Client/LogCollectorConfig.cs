namespace MicroLog.Collector.Client;

/// <summary>
/// Configuration of http client for log collector.
/// </summary>
public class LogCollectorConfig
{
    /// <summary>
    /// Url to log collector endpoint.
    /// </summary>
    public string Url { get; set; }
    /// <summary>
    /// The minimum log level for which the logs should be recorded.
    /// </summary>
    public LogLevel MinimumLevel { get; set; }
}
