using MicroLog.Core.Abstractions;

namespace MicroLog.Core;

/// <summary>
/// Base <see cref="ILogEvent"/> implementation.
/// </summary>
public class LogEvent : ILogEvent
{
    public ILogEventIdentity Identity { get; }
    public string Message { get; set; }
    public DateTime Timestamp { get; init; }
    public LogLevel Level { get; set; }
    public string LevelName => Enum.GetName(typeof(LogLevel), Level);
    public LogException Exception { get; set; }

    private Dictionary<string, ILogProperty> _properties { get; set; } = new();
    public IReadOnlyCollection<ILogProperty> Properties => _properties.Values.ToList().AsReadOnly();

    public LogEvent()
    {
        Identity = new LogIdentity();
        Timestamp = DateTime.Now;
    }

    public LogEvent(ILogEvent log)
    {
        Identity = log.Identity;
        Message = log.Message;
        Timestamp = log.Timestamp;
        Level = log.Level;
        Exception = log.Exception;
        foreach (var prop in log.Properties)
        {
            _properties.Add(prop.Name, prop);
        }
    }

    public LogEvent(ILogEventIdentity identity, string message, DateTime timestamp, LogLevel level, LogException exception, IEnumerable<LogProperty> properties)
    {
        Identity = identity;
        Message = message;
        Timestamp = timestamp;
        Level = level;
        Exception = exception;
        foreach (var prop in properties)
        {
            _properties.Add(prop.Name, prop);
        }
    }

    /// <summary>
    /// If a property with this name does not exist,
    /// adds the given property to the log event.
    /// </summary>
    /// <param name="logProperty">Property to add.</param>
    public void AddProperty(ILogProperty logProperty)
    {
        if (!_properties.ContainsKey(logProperty.Name))
        {
            _properties.Add(logProperty.Name, logProperty);
        }
    }

    public override bool Equals(object obj)
    {
        return obj is ILogEvent @event &&
               Identity.Equals(@event.Identity) &&
               Message == @event.Message &&
               Timestamp == @event.Timestamp &&
               Level == @event.Level &&
               Properties.SequenceEqual(@event.Properties) &&
               Exception is not null ? Exception.Equals(@event.Exception) : true;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Identity, Message, Timestamp, Level, Exception, Properties);
    }
}
