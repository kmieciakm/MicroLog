namespace MicroLog.Sink.Hub;

public interface ILogHubClient
{
    Task ReceiveLog(ILogEvent log);
}