using MicroLog.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MicroLog.Core.Enrichers
{
    /// <summary>
    /// Enrich log event with custom key-value record.
    /// </summary>
    public class ValueEnricher : ILogEnricher
    {
        public string Name { get; init; }
        public string Value { get; init; }

        public ValueEnricher(string name, string value)
        {
            Name = name;
            Value = value;
        }

        /// <inheritdoc />
        public void Enrich(LogEvent log)
        {
            ValueObject obj = new(Value);
            LogProperty property = new()
            {
                Name = Name,
                Value = JsonSerializer.Serialize(obj)
            };
            log.AddProperty(property);
        }

        private record ValueObject(string Value);
    }
}
