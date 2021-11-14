using MicroLog.Core.Abstractions;

namespace MicroLog.Core;

/// <summary>
/// Base <see cref="ILogEventIdentity"/> implementation.
/// </summary>
public class LogIdentity : ILogEventIdentity
{
    public string EventId { get; init; }

    public LogIdentity()
    {
        EventId = Guid.NewGuid().ToString();
    }

    public LogIdentity(string eventId)
    {
        EventId = eventId;
    }

    public static LogIdentity Parse(ILogEventIdentity identity)
        =>  new LogIdentity(identity.EventId);

    public override bool Equals(object obj)
    {
        return obj is ILogEventIdentity identity &&
               EventId == identity.EventId;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(EventId);
    }
}
