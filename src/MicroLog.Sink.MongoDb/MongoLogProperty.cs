namespace MicroLog.Sink.MongoDb;

/// <summary>
/// A MongoDb log property representation.
/// </summary>
internal record MongoLogProperty : ILogProperty
{
    public string Name { get; init; }
    public BsonDocument BsonValue { get; private set; }
    public string Value
    {
        get
        {
            return BsonValue.ToJson();
        }
        init
        {
            BsonValue = BsonDocument.Parse(value);
        }
    }

    public MongoLogProperty(ILogProperty property)
        : this(property.Name, property.Value)
    {
    }

    public MongoLogProperty(string name, string value)
    {
        Name = name;
        Value = value;
    }
}