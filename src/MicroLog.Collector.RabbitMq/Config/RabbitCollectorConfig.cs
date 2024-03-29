﻿namespace MicroLog.Collector.RabbitMq.Config;

/// <summary>
/// Configuration of the RabbitMq endpoint.
/// </summary>
public class RabbitCollectorConfig
{
    public string HostName { get; set; }
    public int Port { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}
