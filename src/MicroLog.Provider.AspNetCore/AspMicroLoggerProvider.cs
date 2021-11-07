using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMicroLogger = MicroLog.Core.Abstractions.ILogger;

namespace MicroLog.Provider.AspNetCore
{
    public sealed class AspMicroLoggerProvider : ILoggerProvider
    {
        private IMicroLogger _Logger { get; }
        private readonly ConcurrentDictionary<string, AspMicroLogger> _loggers = new();

        public AspMicroLoggerProvider(IMicroLogger logger)
        {
            _Logger = logger;
        }

        public ILogger CreateLogger(string categoryName)
            => _loggers.GetOrAdd(categoryName, name => new AspMicroLogger(name, _Logger));

        public void Dispose()
        {
            _loggers.Clear();
        }
    }
}
