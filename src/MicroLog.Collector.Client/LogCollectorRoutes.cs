namespace MicroLog.Collector.Client;

/// <summary>
/// Provides the routes of log collector.
/// </summary>
internal class LogCollectorRoutes
{
    private string _BaseEndpoint { get; init; }

    public LogCollectorRoutes(string url)
    {
        _BaseEndpoint = $"{url}/api/collector";
        Insert = $"{_BaseEndpoint}/insert";
    }

    public string Insert { get; }
}