using MicroLog.Collector.Client;
using MicroLog.Core;
using MicroLog.FunctionalTests.Collector.Fixture;
using System.Diagnostics;
using System.Net.Http;

namespace MicroLog.FunctionalTests.Collector.Cases;

public class CollectorCases : CollectorFixture
{
    [Fact]
    public void DataCollector_Availability()
    {
        using var services = Services().Start();
        Task.Delay(15000).GetAwaiter().GetResult(); // Wait for all services to initialize

        LogCollectorConfig config = GetMicroLogConfig(services);
        LogCollectorClient client = GetMicroLogClient(config);

        Func<Task> logOperation = async () =>
        {
            await client.LogInformationAsync("Test message");
        };
        logOperation.Should().NotThrowAsync();
    }

    [Fact]
    public void DataCollector_Performence()
    {
        using var services = Services().Start();
        Task.Delay(15000).GetAwaiter().GetResult(); // Wait for all services to initialize

        // create test log event
        HttpClient httpClient = new();
        LogEvent logEvent = new()
        {
            Level = LogLevel.Error,
            Message = "Test message - Cannot connect to database.",
            Exception = LogException.Parse(new ArgumentNullException("connectionString", "Value cannot be null."))
        };
        var logContent = JsonSerializer.Serialize(logEvent);
        var logBody = new StringContent(logContent, Encoding.UTF8, "application/json");

        // run request once to warm up, before test
        MakeInsertRequest(httpClient, logBody);

        // perform test
        Stopwatch timer = new();
        var requestsAmount = 1000;
        do
        {
            timer.Start();
            MakeInsertRequest(httpClient, logBody);
            timer.Stop();
            requestsAmount--;
        }
        while (requestsAmount > 0);

        // assert
        timer.ElapsedMilliseconds
            .Should()
            .BeLessThan(3000);

        static HttpResponseMessage MakeInsertRequest(HttpClient httpClient, StringContent body)
        {
            return httpClient
                .PostAsync("https://localhost:3002/api/insert", body)
                .GetAwaiter()
                .GetResult();
        }
    }
}
