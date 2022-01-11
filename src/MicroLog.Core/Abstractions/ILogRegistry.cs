namespace MicroLog.Core.Abstractions;

/// <summary>
/// Provides access to logs persisted in a data storage.
/// </summary>
public interface ILogRegistry
{
    /// <summary>
    /// Returnes the logs from a range.
    /// </summary>
    /// <param name="skip">The number of logs to ommit.</param>
    /// <param name="take">The number of logs to take.</param>
    /// <returns>A task whose result is desired log collection.</returns>
    Task<PaginationResult<ILogEvent>> GetAsync(int skip, int take);
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
}
