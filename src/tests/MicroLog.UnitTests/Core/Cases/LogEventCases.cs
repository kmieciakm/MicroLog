using MicroLog.Core;
using System.Text.Json;

namespace MicroLog.UnitTests.Core.Cases;

public class LogEventCases
{
    [Fact]
    public void Tests_Running()
    {
        true.Should().Be(true);
    }

    [Fact]
    public void LogIdentity_Constructor_Ok()
    {
        var id = new LogIdentity();
        id.EventId.Should().NotBeNullOrEmpty();

        var id2 = new LogIdentity("1111-1111-1111");
        id2.EventId.Should().Be("1111-1111-1111");
    }

    [Fact]
    public void LogIdentity_Equals()
    {
        var id = new LogIdentity("1111-1111-1111");
        var id2 = new LogIdentity("1111-1111-1111");
        id.Should().Be(id2);
        id.Should().NotBeSameAs(id2);
    }

    [Fact]
    public void LogEvent_Constructor_Ok()
    {
        var logEvent = new LogEvent()
        {
            Level = LogLevel.Information,
            Message = "Works !!!"
        };

        logEvent.Timestamp.Should().NotBe(default);
        logEvent.Identity.Should().NotBeNull();
    }

    [Fact]
    public void LogEvent_Equals_Ok()
    {
        var now = DateTime.Now;

        var logEvent = new LogEvent() {
            Identity = new LogIdentity("1111-1111-1111"),
            Level = LogLevel.Information,
            Message = "Works !!!",
            Timestamp = now,
            Exception = LogException.Parse(new ArgumentNullException("Test exception")),
            Properties = new List<LogProperty>() { new() { Name = "Test", Value = "Property" } }
        };

        var logEvent2 = new LogEvent() {
            Identity = new LogIdentity("1111-1111-1111"),
            Level = LogLevel.Information,
            Message = "Works !!!",
            Timestamp = now,
            Exception = LogException.Parse(new ArgumentNullException("Test exception")),
            Properties = new List<LogProperty>() { new() { Name = "Test", Value = "Property" } }
        };

        logEvent.Should().Be(logEvent2);
        logEvent.Should().NotBeSameAs(logEvent2);
    }

    [Fact]
    public void LogEvent_Serialization()
    {
        var logEvent = new LogEvent() {
            Identity = new LogIdentity("6782d198-ee14-4f47-a4ac-e9ac478bdeee"),
            Message = "Works !!!",
            Timestamp = DateTime.Now,
            Level = LogLevel.Information,
            Exception = LogException.Parse(new ArgumentNullException("Test exception")),
            Properties = new List<LogProperty>() { new() { Name = "Test", Value = "Property" } }
        };

        var json = JsonSerializer.Serialize(logEvent);
        var logEventReconstructed = JsonSerializer.Deserialize<LogEvent>(json);

        logEventReconstructed.Should().Be(logEvent);
    }
}
