using BlazorDownloadFile;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MircoLog.Lama.Client;
using MircoLog.Lama.Client.Helpers;
using MircoLog.Lama.Client.Services;
using System;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudBlazorServices();
builder.Services.AddBlazorDownloadFile();

builder.Services.AddSingleton<ILogsStorage, LogsStorage>();
builder.Services.AddSingleton<IAlertService, AlertService>();
builder.Services.AddSingleton<IFilterService, FilterService>();
builder.Services.AddSingleton<IStatisticsService, StatisticsService>();
builder.Services.AddSingleton<IRealTimeStatisticsService, RealTimeStatisticsService>();

await builder.Build().RunAsync();
