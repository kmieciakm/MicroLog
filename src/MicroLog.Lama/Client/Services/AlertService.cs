using MicroLog.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System;

namespace MircoLog.Lama.Client.Services;

interface IAlertService : IRealTimeService
{
    event AlertDelegate OnAlert;
}

class AlertService : RealTimeService, IAlertService
{
    public event AlertDelegate OnAlert;

    public AlertService(NavigationManager navigationManager)
        : base(navigationManager.ToAbsoluteUri("/loghub"))
    {
        HubConnection.On<LogEvent>("ReceiveAlert", (log) =>
        {
            OnAlert?.Invoke(new AlertArgs(log));
        });
    }
}

public delegate void AlertDelegate(AlertArgs arg);

public class AlertArgs : EventArgs
{
    public LogEvent Log { get; }

    public AlertArgs(LogEvent log)
    {
        Log = log;
    }
}