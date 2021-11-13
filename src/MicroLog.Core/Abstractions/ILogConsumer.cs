namespace MicroLog.Core.Abstractions;

/// <summary>
/// Process messages from queue.
/// Consumes log entries and saves them in the permanent data storage.
/// </summary>
public interface ILogConsumer : IDisposable
{
    /// <summary>
    /// Connects to message queue.
    /// Starts receiving and processing of messages.
    /// </summary>
    void Consume();
}
