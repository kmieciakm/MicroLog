namespace MicroLog.Core.Abstractions;

/// <summary>
/// Provides access to logs persisted in a data storage.
/// </summary>
public interface ILogRegistry
{
    /// <summary>
    /// Returns the log matching the identity.
    /// </summary>
    /// <param name="identity">Log event identification.</param>
    /// <returns>A task whose result is desired log.</returns>
    Task<ILogEvent> GetAsync(ILogEventIdentity identity);
    /// <summary>
    /// Returns the collection of logs filter by identities.
    /// </summary>
    /// <param name="identities"></param>
    /// <returns>A task whose result is a collection of desired logs.</returns>
    Task<IEnumerable<ILogEvent>> GetAsync(IEnumerable<ILogEventIdentity> identities);
    /// <summary>
    /// Returns the collection of logs matching the predicate filter.
    /// </summary>
    /// <param name="predicate">Filter function.</param>
    /// <returns>A task whose result is a collection of matching logs.</returns>
    Task<IEnumerable<ILogEvent>> GetAsync(Expression<Func<ILogEvent, bool>> predicate);
}
