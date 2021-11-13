using System.Diagnostics.CodeAnalysis;

namespace MicroLog.Sink.MongoDb.Utils;

internal class DateComparer : IEqualityComparer<DateTime>
{
    public bool Equals(DateTime x, DateTime y)
    {
        return x.ToLongTimeString() == y.ToLongTimeString() &&
            x.ToLongDateString() == y.ToLongDateString();
    }

    public int GetHashCode([DisallowNull] DateTime obj)
    {
        return obj.GetHashCode();
    }
}
