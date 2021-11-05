using MicroLog.Core;
using MicroLog.Core.Abstractions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace MicroLog.Sink.MongoDb
{
    class MongoLogProperty : ILogProperty
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public MongoLogProperty(ILogProperty property)
        {
            Name = property.Name;
            Value = property.Value;
        }

        public MongoLogProperty(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}