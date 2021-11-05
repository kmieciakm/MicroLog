using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Core.Abstractions
{
    public interface ILogProperty
    {
        string Name { get; set; }
        string Value { get; set; }
    }
}
