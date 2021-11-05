using MicroLog.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Core.Enrichers
{
    public class AggregateEnricher : ILogEnricher
    {
        public IEnumerable<ILogEnricher> _Enrichers { get; set; }

        public AggregateEnricher(IEnumerable<ILogEnricher> enrichers)
        {
            _Enrichers = enrichers;
        }

        public void Enrich(LogEvent log)
        {
            foreach (var enricher in _Enrichers)
            {
                enricher.Enrich(log);
            }
        }
    }
}
