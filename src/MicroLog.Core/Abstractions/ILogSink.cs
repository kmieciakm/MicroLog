﻿namespace MicroLog.Core.Abstractions;

/// <summary>
/// Writes log entries to the data destination.
/// </summary>
public interface ILogSink
{
    /// <summary>
    /// Configuration of the sink.
    /// </summary>
    ISinkConfig Config { get; }
    /// <summary>
    /// Inserts log to data storage.
    /// </summary>
    /// <param name="logEvents">Log to insert.</param>
    /// <returns>An operation task.</returns>
    Task InsertAsync(ILogEvent logEvents);
    /// <summary>
    /// Inserts logs collection to data storage.
    /// </summary>
    /// <param name="logEvents">Logs collection to insert.</param>
    /// <returns>An operation task.</returns>
    Task InsertAsync(IEnumerable<ILogEvent> logEvents);
}
