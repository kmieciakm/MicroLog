using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroLog.Collector.Utils
{
    public static class LinqUtils
    {
        public static IEnumerable<T> OrEmptyIfNull<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }
    }
}
