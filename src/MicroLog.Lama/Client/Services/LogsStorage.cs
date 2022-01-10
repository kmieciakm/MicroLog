using MicroLog.Core;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;

namespace MircoLog.Lama.Client.Services;

interface ILogsStorage : IEnumerable<LogEvent>
{
    void Add(LogEvent log);
    void Clear();
    string ToJson();
}

class LogsStorage : ILogsStorage
{
    private LimitedList<LogEvent> _logs = new(100);

    public void Add(LogEvent log) => _logs.Add(log);
    public void Clear() => _logs.Clear();
    public string ToJson() => JsonSerializer.Serialize(_logs, new JsonSerializerOptions { WriteIndented = true });

    public IEnumerator<LogEvent> GetEnumerator() => _logs.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private class LimitedList<T> : LinkedList<T>
    {
        public int Limit { get; set; }

        public LimitedList(int limit)
        {
            Limit = limit;
        }

        public void Add(T item)
        {
            while (Count >= Limit)
            {
                RemoveLast();
            }
            AddFirst(item);
        }
    }
}
