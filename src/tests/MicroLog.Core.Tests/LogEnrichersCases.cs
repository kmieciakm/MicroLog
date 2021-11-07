using FluentAssertions;
using MicroLog.Core.Abstractions;
using MicroLog.Core.Enrichers;
using Microsoft.AspNetCore.Http;
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

        [Fact]
        public void Log_HttpEnricher_Success()
        {
            LogEvent log = new()
            {
                Level = LogLevel.Error,
                Message = "Bad request !!!"
            };

            HttpContext httpContext = new DefaultHttpContext();
            httpContext.Request.Method = "POST";
            httpContext.Response.StatusCode = 400;

            HttpContextAccessor accessor = new();
            accessor.HttpContext = httpContext;

            HttpContextEnricher enricher = new(accessor);
            enricher.Enrich(log);

            LogProperty property = new()
            {
                Name = "HttpContext",
                Value = "{\"Request\":{\"Url\":\":///\",\"Method\":\"POST\"},\"Response\":{\"StatusCode\":\"400\"}}"
            };

            log.Properties.Should().HaveCount(1);
            log.Properties.First().Should().Be(property);
        }

        [Fact]
        public void Log_HttpEnricher_EmptyAccessor()
        {
            LogEvent log = new()
            {
                Level = LogLevel.Warning,
                Message = "No HttpContext !!!"
            };

            HttpContextAccessor accessor = new();
            HttpContextEnricher enricher = new(accessor);
            enricher.Enrich(log);

            log.Properties.Should().HaveCount(0);
        }
    }
}