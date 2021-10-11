using System;
using Microsoft.Extensions.Logging;
using IMicroLogger = MicroLog.Core.ILogger;

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
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }
    }
}
