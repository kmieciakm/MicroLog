using MicroLog.Core.Abstractions;
using System.Text.Json.Serialization;

namespace MicroLog.Core;

public class LogIdentityConverter : JsonConverter<ILogEventIdentity>
{
    public override LogIdentity Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        string eventId = null;
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                return new LogIdentity(eventId);
            }

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                string propName = (reader.GetString() ?? "").ToLower();
                reader.Read();

                switch (propName)
                {
                    case var _ when propName.Equals(nameof(LogIdentity.EventId).ToLower()):
                        eventId = reader.GetString();
                        break;
                }
            }
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, ILogEventIdentity value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString(nameof(LogIdentity.EventId), value.EventId);
        writer.WriteEndObject();
    }
}

public class LogPropertiesConverter : JsonConverter<IEnumerable<ILogProperty>>
{
    public override IEnumerable<ILogProperty> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException();
        }

        List<ILogProperty> properties = new();
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
            {
                return properties;
            }

            string name = null;
            string value = null;
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject)
                    {
                        properties.Add(new LogProperty() { Name = name, Value = value });
                        break;
                    }

                    string propName = (reader.GetString() ?? "").ToLower();
                    reader.Read();

                    switch (propName)
                    {
                        case var _ when propName.Equals(nameof(LogProperty.Name).ToLower()):
                            name = reader.GetString();
                            break;
                        case var _ when propName.Equals(nameof(LogProperty.Value).ToLower()):
                            value = reader.GetString();
                            break;
                    }
                }
            }
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, IEnumerable<ILogProperty> value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (var property in value)
        {
            writer.WriteStartObject();
            writer.WriteString(nameof(property.Name), property.Name);
            writer.WriteString(nameof(property.Value), property.Value);
            writer.WriteEndObject();
        }
        writer.WriteEndArray();
    }
}

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
