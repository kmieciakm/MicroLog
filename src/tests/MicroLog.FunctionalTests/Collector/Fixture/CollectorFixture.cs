using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Model.Common;
using Ductus.FluentDocker.Services;
using Ductus.FluentDocker.Services.Impl;
using MicroLog.Collector.Client;

namespace MicroLog.FunctionalTests.Collector.Fixture;

public abstract class CollectorFixture
{
    private static string dockerComposeFile = Path.Combine(Directory.GetCurrentDirectory(), (TemplateString)"collector/fixture/docker-compose.yml");

    protected static ICompositeService Services()
    {
        return (DockerComposeCompositeService)new Builder()
            .UseContainer()
            .ExposePort(3002, 3002)
            .ExposePort(5673, 5673)
            .ExposePort(15673, 15673)
            .ExposePort(27018, 27018)
            .UseCompose()
            .FromFile(dockerComposeFile)
            .RemoveOrphans()
            .Build();
    }

    protected static LogCollectorConfig GetMicroLogConfig(ICompositeService services)
        => new()
        {
            Url = $"https://localhost:3002",
            MinimumLevel = Core.LogLevel.Trace
        };

    protected static LogCollectorClient GetMicroLogClient(LogCollectorConfig config)
        => new LogCollectorClient(config);

    [Fact]
    public void Services_Running()
    {
        using var services = Services().Start();
        Assert.Equal(ServiceRunningState.Running, services.State);
    }
}
