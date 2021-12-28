using MircoLog.Lama.Shared.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MircoLog.Lama.Client.Services;

interface IFilterService
{
    Task<IEnumerable<Filter>> GetAsync();
    Task EditAsync(Filter filter);
    Task CreateAsync(Filter filter);
}

class FilterService : IFilterService
{
    private HttpClient _HttpClient { get; set; }

    public FilterService(HttpClient httpClient)
    {
        _HttpClient = httpClient;
    }

    public async Task<IEnumerable<Filter>> GetAsync()
    {
        return await _HttpClient.GetFromJsonAsync<IEnumerable<Filter>>("/filters");
    }

    public async Task EditAsync(Filter filter)
    {
        var body = JsonSerializer.Serialize(filter);
        var content = new StringContent(body, Encoding.UTF8, "application/json");
        var response = await _HttpClient.PatchAsync("/filters/edit", content);

        await ThrowIfNotSucceded(response);
    }

    public async Task CreateAsync(Filter filter)
    {
        var body = JsonSerializer.Serialize(filter);
        var content = new StringContent(body, Encoding.UTF8, "application/json");
        var response = await _HttpClient.PostAsync("/filters/save", content);

        await ThrowIfNotSucceded(response);
    }

    private static async Task ThrowIfNotSucceded(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    var badRequest = await response.Content?.ReadAsStringAsync();
                    throw new ServiceException(badRequest, ExceptionCause.BAD_REQUEST);
                case HttpStatusCode.InternalServerError:
                    throw new ServiceException("Unexpected error has occured.", ExceptionCause.UNKNOWN);
                default:
                    var error = await response.Content?.ReadAsStringAsync();
                    throw new ServiceException(error, ExceptionCause.UNKNOWN);
            }
        }
    }
}