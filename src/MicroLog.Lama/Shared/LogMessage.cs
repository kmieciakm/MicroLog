namespace MircoLog.Lama.Shared;

public class LogMessage
{
    public string? Type { get; set; }
    public string? Message { get; set; }

    public override string ToString()
    {
        return $"{Type}: {Message}";
    }
}
