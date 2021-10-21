using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Driver.MongoDb.Utils
{
    class DateComparer : IEqualityComparer<DateTime>
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
}
