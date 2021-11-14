using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using IMicroLogger = MicroLog.Core.Abstractions.ILogger;

namespace MicroLog.Provider.AspNetCore;

/// <summary>
/// Provider that can create instances of the <see cref="AspMicroLogger"/>.
/// </summary>
public sealed class AspMicroLoggerProvider : ILoggerProvider
{
    private IMicroLogger _Logger { get; }

    // Internally, the ConcurrentDictionary uses locking to make it thread safe
    // for most methods, but GetOrAdd does not lock while valueFactory is running. 
    // Lazy<T> solves the problem.
    private readonly ConcurrentDictionary<string, Lazy<AspMicroLogger>> _loggers = new();

    public AspMicroLoggerProvider(IMicroLogger logger)
    {
        _Logger = logger;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, name =>
                new Lazy<AspMicroLogger>(new AspMicroLogger(name, _Logger))).Value;
    }
    public void Dispose()
    {
        _loggers.Clear();
    }
}
