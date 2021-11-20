namespace MicroLog.Core.Abstractions;

/// <summary>
/// Asynchronous logger. 
/// </summary>
public interface ILogger
{
    void AddEnricher(ILogEnricher enricher);
    bool ShouldLog(LogLevel level);
    Task LogAsync(LogLevel level, string message, LogException exception = null, ILogEnricher enricher = null);
    Task LogTraceAsync(string message);
    Task LogTraceAsync(string message, ILogEnricher enricher);
    Task LogDebugAsync(string message);
    Task LogDebugAsync(string message, ILogEnricher enricher);
    Task LogInformationAsync(string message);
    Task LogInformationAsync(string message, ILogEnricher enricher);
    Task LogWarningAsync(string message, LogException exception = null);
    Task LogWarningAsync(string message, ILogEnricher enricher, LogException exception = null);
    Task LogErrorAsync(string message, LogException exception = null);
    Task LogErrorAsync(string message, ILogEnricher enricher, LogException exception = null);
    Task LogCriticalAsync(string message, LogException exception = null);
    Task LogCriticalAsync(string message, ILogEnricher enricher, LogException exception = null);
}
