using MicroLog.Core;
using System;
using System.Collections.Generic;

namespace MircoLog.Lama.Client.Services;

class Filter
{
    public string Name { get; set; }
    public string Query { get; set; }
}

public class FilterResponse
{
    public Logs Logs { get; set; }
}

public class Logs
{
    public long? TotalCount { get; set; }
    public List<LogItem> Items { get; set; }
}

public class LogItem
{
    public LogIdentity Identity { get; set; }
    public DateTime? Timestamp { get; set; }
    public string Message { get; set; }
    public string Level { get; set; }
    public string LevelName { get; set; }
    public LogException Exception { get; set; }
    public List<LogProperty> Properties { get; set; }
}