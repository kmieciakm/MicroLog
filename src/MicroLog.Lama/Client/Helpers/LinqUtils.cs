using System.Collections.Generic;
using System.Linq;

namespace MircoLog.Lama.Client.Helpers;

internal static class LinqUtils
{
    public static IEnumerable<T> OrEmptyIfNull<T>(this IEnumerable<T> source)
    {
        return source ?? Enumerable.Empty<T>();
    }
}
