using MicroLog.Core.Abstractions;
using MicroLog.Core.JsonConverters;

namespace MicroLog.Core;

/// <summary>
/// Base <see cref="ILogEvent"/> implementation.
/// </summary>
public class LogEvent : ILogEvent
{
    [JsonConverter(typeof(LogIdentityConverter))]
    public ILogEventIdentity Identity { get; init; }
    public string Message { get; set; }
    public DateTime Timestamp { get; init; }
    public LogLevel Level { get; set; }
    public string LevelName => Enum.GetName(typeof(LogLevel), Level);
    public LogException Exception { get; set; }
    private Dictionary<string, ILogProperty> _properties { get; set; } = new();

    [JsonConverter(typeof(LogPropertiesConverter))]
    public IEnumerable<ILogProperty> Properties
    {
        get
        {
            return _properties.Values;
        }

        init
        {
            foreach (var property in value)
            {
                _properties.Add(property.Name, property);
            }
        }
    }

    public LogEvent()
    {
        Identity = new LogIdentity();
        Timestamp = DateTime.UtcNow;
    }

    /// <summary>
    /// Creates new <see cref="LogEvent"/> based on prototype.
    /// </summary>
    /// <param name="log">Propotype</param>
    /// <returns>New instace of a LogEvent.</returns>
    public static LogEvent Parse(ILogEvent log)
        => new()
        {
            Identity = log.Identity,
            Message = log.Message,
            Timestamp = log.Timestamp,
            Level = log.Level,
            Exception = log.Exception,
            Properties = log.Properties
        };

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
