using MicroLog.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Polly;
using System;
using System.Threading.Tasks;

namespace MircoLog.Lama.Client.Services;

public interface IAlertService : IAsyncDisposable
{
    event AlertDelegate OnAlert;
    bool IsConnected { get; }

    Task Run();
    Task Connect();
    Task Disconnect();
}

public class AlertService : IAlertService
{
    private HubConnection _hubConnection;

    public bool IsConnected =>
        _hubConnection?.State == HubConnectionState.Connected;

    public event AlertDelegate OnAlert;

    public AlertService(NavigationManager navigationManager)
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(navigationManager.ToAbsoluteUri("/loghub"))
            .Build();

        _hubConnection.On<LogEvent>("ReceiveAlert", (log) =>
        {
            OnAlert?.Invoke(new AlertArgs(log));
        });
    }
    public async Task Run()
    {
        var pauseBetweenFailures = TimeSpan.FromSeconds(5);
        var retryPolicy = Policy
             .Handle<Exception>()
             .WaitAndRetryForeverAsync(_ => pauseBetweenFailures);

        await retryPolicy.ExecuteAsync(async () =>
        {
            await Connect();
        });
    }

    public async Task Connect()
    {
        await _hubConnection.StartAsync();
    }

    public async Task Disconnect()
    {
        await _hubConnection.StopAsync();
    }

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection is not null)
        {
            await Disconnect();
            await _hubConnection.DisposeAsync();
        }
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