namespace MicroLog.Core.Abstractions;

/// <summary>
/// Sends log events to the Message Queue.
/// </summary>
public interface ILogPublisher : IDisposable
{
    /// <summary>
    /// Configuration of the publisher.
    /// </summary>
    IPublisherConfig Config { get; }
    /// <summary>
    /// Connects to Message Queue.
    /// </summary>
    void Connect();
    /// <summary>
    /// Publishes given log to queue.
    /// </summary>
    /// <param name="logEvent">Log event to send.</param>
    /// <returns>An operation task.</returns>
    Task PublishAsync(ILogEvent logEvent);
    /// <summary>
    /// Sends each log from batch separately to queue.
    /// </summary>
    /// <param name="logEvents">Log events to send.</param>
    /// <returns>An operation task.</returns>
    Task PublishAsync(IEnumerable<ILogEvent> logEvents);
}
