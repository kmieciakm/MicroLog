using MicroLog.Core.Abstractions;

namespace MicroLog.Core.JsonConverters;

internal class LogIdentityConverter : JsonConverter<ILogEventIdentity>
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
