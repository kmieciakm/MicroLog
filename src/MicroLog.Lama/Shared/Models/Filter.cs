using MongoDB.Bson.Serialization.Attributes;

namespace MircoLog.Lama.Shared.Models;

/* TODO: Delete MongoDb specific code, and introduce separate model */

[BsonIgnoreExtraElements]
public class Filter
{
    public string Name { get; set; }
    public string Query { get; set; } = string.Empty;
}
