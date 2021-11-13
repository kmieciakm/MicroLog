using MicroLog.Core;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;

namespace MircoLog.Lama.Client.Services;

public interface ILogsStorage : IEnumerable<LogEvent>
{
    void Add(LogEvent log);
    void Clear();
    string ToJson();
}

public class LogsStorage : ILogsStorage
{
    private List<LogEvent> _logs = new();

    public void Add(LogEvent log) => _logs.Add(log);
    public void Clear() => _logs.Clear();
    public string ToJson() => JsonSerializer.Serialize(_logs);

    public IEnumerator<LogEvent> GetEnumerator() => _logs.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
