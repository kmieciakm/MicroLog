using Microsoft.Extensions.Logging;
using IMicroLogger = MicroLog.Core.Abstractions.ILogger;
using IMicroLogLevel = MicroLog.Core.LogLevel;

namespace MicroLog.Provider.AspNetCore;

/// <summary>
/// Wrapper around core MicroLog ILogger,
/// that implements default Asp.Net logger interface <see cref="ILogger"/>.
/// </summary>
public class AspMicroLogger : ILogger
{
    private string _Name { get; }
    public IMicroLogger _Logger { get; private set; }

    public AspMicroLogger(string name, IMicroLogger logger)
    {
        _Name = name;
        _Logger = logger;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        var level = ConvertLogLevel(logLevel);
        _Logger.LogAsync(level, formatter(state, exception), new Core.LogException(exception));
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        var level = ConvertLogLevel(logLevel);
        return _Logger.ShouldLog(level);
    }

    public IDisposable BeginScope<TState>(TState state) => default;

    private static IMicroLogLevel ConvertLogLevel(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Trace => IMicroLogLevel.Trace,
            LogLevel.Debug => IMicroLogLevel.Debug,
            LogLevel.Information => IMicroLogLevel.Information,
            LogLevel.Warning => IMicroLogLevel.Warning,
            LogLevel.Error => IMicroLogLevel.Error,
            LogLevel.Critical => IMicroLogLevel.Critical,
            LogLevel.None => IMicroLogLevel.None,
            _ => IMicroLogLevel.None
        };
    }
}
