using MicroLog.Core.Abstractions;
using Microsoft.AspNetCore.Http;

namespace MicroLog.Core.Enrichers;

/// <summary>
/// Enriches the log with the environment data.
/// </summary>
public class EnvironmentEnricher : ILogEnricher
{
    public string Name { get; set; } = "Service Environment";

    public EnvironmentEnricher(string name)
    {
        Name = name;
    }

    public EnvironmentEnricher() { }

    public void Enrich(LogEvent log)
    {
        ServiceEnvironment env = new()
        {
            MachineName = Environment.MachineName,
            OSVersion = Environment.OSVersion.ToString(),
            ProcessId = Environment.ProcessId.ToString()
        };

        LogProperty property = new()
        {
            Name = Name,
            Value = JsonSerializer.Serialize(env)
        };
        log.AddProperty(property);
    }

    private class ServiceEnvironment
    {
        public string MachineName { get; set; }
        public string OSVersion { get; set; }
        public string ProcessId { get; set; }
    }
}

internal class EnvironmentEnricherMiddleware
{
    private RequestDelegate _next { get; set; }

    public EnvironmentEnricherMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ILogger logger)
    {
        var enricher = new EnvironmentEnricher();
        logger.AddEnricher(enricher);
      
        await _next.Invoke(context);
    }
}
