using System.Collections.Generic;

namespace MircoLog.Lama.Client.Models;

public class FilterResponse
{
    public Logs Logs { get; set; }
}

public class Logs
{
    public long? TotalCount { get; set; }
    public List<LogItem> Items { get; set; }
}
