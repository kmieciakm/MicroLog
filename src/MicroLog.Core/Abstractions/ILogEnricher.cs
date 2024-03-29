﻿namespace MicroLog.Core.Abstractions;

/// <summary>
///  Use to add additional information to log records.
/// </summary>
public interface ILogEnricher
{
    /// <summary>
    /// Adds additional data to log properties.
    /// </summary>
    /// <param name="log">Log event to enrich with data.</param>
    void Enrich(LogEvent log);
}
