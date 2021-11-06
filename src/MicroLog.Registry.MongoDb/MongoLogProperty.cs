using MicroLog.Core.Abstractions;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroLog.Sink.MongoDb
{
    record MongoLogProperty : ILogProperty
    {
        public string Name { get; init; }

        private string _bsonValue;
        public string Value
        {
            get
            {
                return _bsonValue;
            }
            init
            {
                _bsonValue = BsonDocument.Parse(value).ToJson();
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