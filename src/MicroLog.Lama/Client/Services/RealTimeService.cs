using Microsoft.AspNetCore.SignalR.Client;
using Polly;
using System;
using System.Threading.Tasks;

namespace MircoLog.Lama.Client.Services;

interface IRealTimeService : IAsyncDisposable
{
    bool IsConnected { get; }
    Task Run();
    Task Connect();
    Task Disconnect();
}

abstract class RealTimeService : IRealTimeService
{
    protected HubConnection HubConnection { get; set; }

    public RealTimeService(Uri hubUri)
    {
        HubConnection = new HubConnectionBuilder()
            .WithUrl(hubUri)
            .Build();
    }

    public bool IsConnected =>
        HubConnection?.State == HubConnectionState.Connected;

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
        => await HubConnection.StartAsync();

    public async Task Disconnect()
        => await HubConnection.StopAsync();

    public async ValueTask DisposeAsync()
    {
        if (HubConnection is not null)
        {
            await Disconnect();
            await HubConnection.DisposeAsync();
        }
    }
}
