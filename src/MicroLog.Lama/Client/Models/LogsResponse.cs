using System.Collections.Generic;

namespace MircoLog.Lama.Client.Models;

public class LogsResponse
{
    public long? TotalCount { get; set; }
    public List<LogItem> Items { get; set; }
}
