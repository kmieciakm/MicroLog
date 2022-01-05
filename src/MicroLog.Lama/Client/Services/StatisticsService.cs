using MicroLog.Core.Statistics;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace MircoLog.Lama.Client.Services;

public interface IStatisticsService
{
    Task<LogsStatistics> GetDailyStatisticsAsync();
    Task<LogsStatistics> GetTotalStatisticsAsync();
}

public class StatisticsService : IStatisticsService
{
    private HttpClient _HttpClient { get; set; }

    public StatisticsService(HttpClient httpClient)
    {
        _HttpClient = httpClient;
    }

    public async Task<LogsStatistics> GetDailyStatisticsAsync()
    {
        var response = await _HttpClient.GetAsync("api/statistics/daily");
        var body = await response.Content.ReadAsStringAsync();
        LogsStatistics stats = JsonSerializer.Deserialize<LogsStatistics>(body,
            new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        return stats;
    }

    public async Task<LogsStatistics> GetTotalStatisticsAsync()
    {
        return await _HttpClient.GetFromJsonAsync<LogsStatistics>("api/statistics/total");
    }
}
