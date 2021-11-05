using MicroLog.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MicroLog.Core.Enrichers
{
    public class ValueEnricher : ILogEnricher
    {
        public string Name { get; init; }
        public string Value { get; init; }

        public ValueEnricher(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public void Enrich(LogEvent log)
        {
            ValueObject obj = new(Value);
            LogProperty property = new()
            {
                Name = Name,
                Value = JsonSerializer.Serialize(obj)
            };
            log.Enrich(property);
        }

        private record ValueObject(string Value);
    }
}
