using System;
using Microsoft.Extensions.Logging;
using IMicroLogger = MicroLog.Core.Abstractions.ILogger;
using IMicroLogLevel = MicroLog.Core.LogLevel;

namespace MicroLog.Provider.AspNetCore
{
    public class MicroLogProvider<TCategoryName> : ILogger<TCategoryName>
    {
        public IMicroLogger _Logger { get; set; }

        public MicroLogProvider(IMicroLogger logger)
        {
            _Logger = logger;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var level = ConvertLogLevel(logLevel);
            _Logger.LogAsync(level, formatter(state, exception), exception);
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
}
