using MicroLog.Core.Abstractions;

namespace MicroLog.Core.Enrichers;

/// <summary>
/// Composite log enricher.
/// </summary>
public class AggregateEnricher : ILogEnricher
{
    public IEnumerable<ILogEnricher> _Enrichers { get; set; }

    public AggregateEnricher(IEnumerable<ILogEnricher> enrichers)
    {
        _Enrichers = enrichers;
    }

    /// <inheritdoc />
    public void Enrich(LogEvent log)
    {
        foreach (var enricher in _Enrichers)
        {
            enricher.Enrich(log);
        }
    }
}
