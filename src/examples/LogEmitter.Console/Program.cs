using MicroLog.Collector.Client;
using MicroLog.Core;
using MicroLog.Core.Enrichers;
using System;
using System.Linq;
using System.Threading.Tasks;

LogCollectorConfig config = new()
{
    Url = $"https://localhost:3001",
    MinimumLevel = LogLevel.Debug
};

LogCollectorClient logger = new(config);

ValueEnricher machineEnricher = new("OS", Environment.OSVersion.ToString());
logger.AddEnricher(machineEnricher);

var ex = LogException.Parse(
    new ArgumentNullException("connectionString", "Value cannot be null."));

var amount = 100000;
await Parallel.ForEachAsync(Enumerable.Range(0, amount / 5), async (_, _) =>
{
    await logger.LogDebugAsync("Server is listening  on port 5000.");
    await logger.LogInformationAsync("Uploaded image: sample.png.");
    await logger.LogWarningAsync("Third invalid login attempt. User id: 4521-543922-2135");
    await logger.LogErrorAsync("Cannot connect to database.", ex);
    await logger.LogCriticalAsync("Background service execution fails !!!");
});