namespace MicroLog.Core.Abstractions;

/// <summary>
/// Configuration of ILogPublisher.
/// </summary>
public interface IPublisherConfig
{
    /// <summary>
    /// The set containing the names of queues to which ILogPublisher should send logs.
    /// </summary>
    /// <returns>Queues' names</returns>
    IEnumerable<string> GetQueues();
}
