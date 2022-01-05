using MicroLog.Core;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MircoLog.Lama.Client.Models;

public class LogItem
{
    public LogIdentity Identity { get; set; }
    public DateTime? Timestamp { get; set; }
    public string Message { get; set; }
    public string ShortMessage
    {
        get
        {
            if (Message.Length >= 97)
            {
                return $"{Message.Substring(0, 96)} ...";
            }
            return Message;
        }
    }
    public string Level { get; set; }
    public string LevelName { get; set; }
    public LogException Exception { get; set; }
    public List<LogProperty> Properties { get; set; }

    public Color GetSeverityColor()
    {
        var level = Enum.Parse<LogLevel>(Level, true);
        return level switch
        {
            >= LogLevel.Error => Color.Error,
            LogLevel.Warning => Color.Warning,
            LogLevel.Information => Color.Primary,
            <= LogLevel.Debug => Color.Dark
        };
    }

    public static LogItem Parse(LogEvent logEvent)
    {
        return new LogItem()
        {
            Identity = logEvent.Identity as LogIdentity,
            Level = logEvent.Level.ToString(),
            LevelName = Enum.GetName(logEvent.Level),
            Message = logEvent.Message,
            Timestamp = logEvent.Timestamp,
            Exception = logEvent.Exception,
            Properties = logEvent.Properties.Select(p => p as LogProperty).ToList()
        };
    }
}