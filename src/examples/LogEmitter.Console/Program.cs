﻿using MicroLog.Collector.Client;
using MicroLog.Core;
using System;

LogCollectorConfig config = new()
{
    Url = $"https://localhost:3001",
    MinimumLevel = LogLevel.Debug
};

LogCollectorClient client = new(config);

int i = 0;
do
{
    await client.LogDebugAsync("Server is listening  on port 5000.");
    await client.LogInformationAsync("Uploaded image: sample.png.");
    await client.LogWarningAsync("Third invalid login attempt. User id: 4521-543922-2135");
    await client.LogErrorAsync("Cannot connect to database.", LogException.Parse(new ArgumentNullException("connectionString", "Value cannot be null.")));
    await client.LogCriticalAsync("Background service execution fails !!!");
    i++;
} while (i < 2000);