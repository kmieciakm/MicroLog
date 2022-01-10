using MicroLog.Core.Statistics;
using MircoLog.Lama.Client.Models;
using MircoLog.Lama.Shared.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MircoLog.Lama.Client.Services;

interface IStatisticsService
{
    Task<DailyStatistics> GetDailyStatisticsAsync();
    Task<TotalStatistics> GetTotalStatisticsAsync();
    Task<IEnumerable<LogItem>> GetLastErrorsAsync();
}

class StatisticsService : IStatisticsService
{
    private HttpClient _HttpClient { get; set; }
    private IFilterService _FilterService { get; set; }

    public StatisticsService(HttpClient httpClient, IFilterService filterService)
    {
        _HttpClient = httpClient;
        _FilterService = filterService;
    }

    public async Task<DailyStatistics> GetDailyStatisticsAsync()
    {
        return await _HttpClient.GetFromJsonAsync<DailyStatistics>("api/statistics/daily");
    }

    public async Task<TotalStatistics> GetTotalStatisticsAsync()
    {
        return await _HttpClient.GetFromJsonAsync<TotalStatistics>("api/statistics/total");
    }

    public async Task<IEnumerable<LogItem>> GetLastErrorsAsync()
    {
        var query = await _HttpClient.GetStringAsync("queries/errorsQuery.txt");
        var logs = await _FilterService.Execute(
            new Filter()
            {
                Name = "LastErrors",
                Query = query
            }
        );
        return logs.Items;
    }
}
