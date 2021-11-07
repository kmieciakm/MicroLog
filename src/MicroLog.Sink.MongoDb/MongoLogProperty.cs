using MicroLog.Core.Abstractions;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroLog.Sink.MongoDb
{
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
}