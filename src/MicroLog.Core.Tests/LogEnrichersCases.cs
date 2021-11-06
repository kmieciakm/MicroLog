using FluentAssertions;
using MicroLog.Core.Abstractions;
using MicroLog.Core.Enrichers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MicroLog.Core.Tests
{
    public class LogEnrichersCases
    {
        [Fact]
        public void Tests_Running()
        {
            true.Should().Be(true);
        }

        [Fact]
        public void Log_ValueEnricher_Success()
        {
            LogEvent log = new()
            {
                Level = LogLevel.Information,
                Message = "Works !!!"
            };

            ValueEnricher enricher = new("Test", "TestValue");
            enricher.Enrich(log);

            LogProperty property = new() { Name = "Test", Value = "{\"Value\":\"TestValue\"}" };

            log.Properties.Should().HaveCount(1);
            log.Properties.First().Should().Be(property);
        }

        [Fact]
        public void Log_AggregateEnricher_Success()
        {
            LogEvent log = new()
            {
                Level = LogLevel.Information,
                Message = "Works !!!"
            };

            ValueEnricher enricher1 = new("Test_1", "TestValue_1");
            ValueEnricher enricher2 = new("Test_2", "TestValue_2");
            List<ILogEnricher> enrichers = new() { enricher1, enricher2 };

            AggregateEnricher aggregateEnricher = new(enrichers);
            aggregateEnricher.Enrich(log);

            log.Properties.Should().HaveCount(2);
        }
    }
}
