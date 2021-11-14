using MicroLog.Core.Abstractions;

namespace MicroLog.Core.JsonConverters;

internal class LogPropertiesConverter : JsonConverter<IEnumerable<ILogProperty>>
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
