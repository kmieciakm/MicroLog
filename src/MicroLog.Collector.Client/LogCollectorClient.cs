namespace MicroLog.Collector.Client;

/// <summary>
/// <see cref="ILogger"/> that sends logs via HTTP.
/// Encapsulates HTTP client for log collector.
/// </summary>
public class LogCollectorClient : ILogger
{
    private static readonly HttpClient _httpClient = new HttpClient();

    private LogCollectorConfig _Config { get; }
    private LogCollectorRoutes _Routes { get; }

    public LogCollectorClient(IOptions<LogCollectorConfig> configOptions)
        : this(configOptions.Value)
    {
    }

    public LogCollectorClient(LogCollectorConfig config)
    {
        _Config = config;
        _Routes = new LogCollectorRoutes(config.Url);
    }

    public bool ShouldLog(LogLevel level)
        => level >= _Config.MinimumLevel;

    public async Task LogAsync(LogLevel level, string message, LogException exception = null)
    {
        if (ShouldLog(level))
        {
            var logEvent = new LogEvent()
            {
                Level = level,
                Message = message,
                Exception = exception
            };
            var content = JsonSerializer.Serialize(logEvent);
            var body = new StringContent(content, Encoding.UTF8, "application/json");
            await _httpClient.PostAsync(_Routes.Insert, body);
        }
    }

    public async Task LogTraceAsync(string message)
        => await LogAsync(LogLevel.Trace, message);

    public async Task LogDebugAsync(string message)
        => await LogAsync(LogLevel.Debug, message);

    public async Task LogInformationAsync(string message)
        => await LogAsync(LogLevel.Information, message);

    public async Task LogWarningAsync(string message, LogException exception = null)
        => await LogAsync(LogLevel.Warning, message, exception);

    public async Task LogErrorAsync(string message, LogException exception = null)
        => await LogAsync(LogLevel.Error, message, exception);

    public async Task LogCriticalAsync(string message, LogException exception = null)
        => await LogAsync(LogLevel.Critical, message, exception);
}
