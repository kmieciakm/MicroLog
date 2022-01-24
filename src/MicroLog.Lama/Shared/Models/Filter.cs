using MongoDB.Bson.Serialization.Attributes;

namespace MircoLog.Lama.Shared.Models;

/* TODO: Delete MongoDb specific code, and introduce separate model */

/// <summary>
/// Filter, used to query the log
/// with utilization of the GraphQl requests.
/// </summary>
[BsonIgnoreExtraElements]
public class Filter
{
    /// <summary>
    /// The identification of a filter.
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// GraphQl request.
    /// </summary>
    public string Query { get; set; } = string.Empty;
}
