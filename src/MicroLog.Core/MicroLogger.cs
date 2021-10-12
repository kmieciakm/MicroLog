using MicroLog.Core.Config;
using MicroLog.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Core
{
    public class MicroLogger : ILogger
    {
        private ILogSink _LogSink { get; set; }
        private MicroLogConfig _Config { get; set; }

        public MicroLogger(ILogSink logSink)
        {
            _LogSink = logSink;
        }

        public MicroLogger(ILogSink logSink, MicroLogConfig config) : this(logSink)
        {
            _Config = config;
        }

        public bool ShouldLog(LogLevel level)
            => level >= _Config.MinimumLevel;

        private async Task LogAsync(LogLevel level, string message, Exception exception = null)
        {
            if (ShouldLog(level))
            {
                var logEvent = new LogEvent(_Config.ServiceName)
                {
                    Level = level,
                    Message = message,
                    Exception = exception
                };
                await _LogSink.InsertAsync(logEvent);
            }
        }

        public async Task LogTraceAsync(string message)
            => await LogAsync(LogLevel.Trace, message);

        public async Task LogDebugAsync(string message)
            => await LogAsync(LogLevel.Debug, message);

        public async Task LogInformationAsync(string message)
            => await LogAsync(LogLevel.Information, message);

        public async Task LogWarningAsync(string message, Exception exception = null)
            => await LogAsync(LogLevel.Warning, message, exception);

        public async Task LogErrorAsync(string message, Exception exception = null)
            => await LogAsync(LogLevel.Error, message, exception);

        public async Task LogCriticalAsync(string message, Exception exception = null)
            => await LogAsync(LogLevel.Critical, message, exception);
    }
}
