using MicroLog.Core.Abstractions;

namespace MicroLog.Core.Enrichers;

/// <summary>
/// Composite log enricher.
/// </summary>
public class AggregateEnricher : ILogEnricher
{
    public List<ILogEnricher> _Enrichers { get; set; }

    public AggregateEnricher(IEnumerable<ILogEnricher> enrichers)
    {
        _Enrichers = enrichers.ToList();
    }

    public AggregateEnricher()
    {
        _Enrichers = new List<ILogEnricher>();
    }

    /// <summary>
    /// Appends given enricher to itself.
    /// </summary>
    /// <param name="enricher">Enricher to append.</param>
    public void Add(ILogEnricher enricher)
        => _Enrichers.Add(enricher);

    /// <inheritdoc />
    public void Enrich(LogEvent log)
    {
        foreach (var enricher in _Enrichers)
        {
            enricher.Enrich(log);
        }
    }
}
