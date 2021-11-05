using MicroLog.Core;
using MicroLog.Core.Abstractions;
using MicroLog.Core.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MicroLog.Collector.Client
{
    public class MicroLogClient : ILogger
    {
        private HttpClient _HttpClient { get; set; }
        private MicroLogConfig _Config { get; set; }
        private MicroLogRoutes _Routes { get; set; }

        public MicroLogClient(MicroLogConfig config)
        {
            _Config = config;
            _Routes = new MicroLogRoutes(config.Url);
        }

        public bool ShouldLog(LogLevel level)
            => level >= _Config.MinimumLevel;

        public async Task LogAsync(LogLevel level, string message, Exception exception = null)
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
                await _HttpClient.PostAsync(_Routes.Insert, body);
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
