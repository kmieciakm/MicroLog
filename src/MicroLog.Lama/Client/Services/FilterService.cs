using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.AspNetCore.Components;
using MircoLog.Lama.Client.Models;
using MircoLog.Lama.Shared.Models;
using System;
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
    Task DeleteAsync(Filter filter);
    Task<LogsResponse> Execute(Filter filter);
}

class FilterService : IFilterService
{
    private NavigationManager _NavigationManager{ get; set; }
    private HttpClient _HttpClient { get; set; }

    public FilterService(HttpClient httpClient, NavigationManager navigationManager)
    {
        _HttpClient = httpClient;
        _NavigationManager = navigationManager;
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

    public async Task DeleteAsync(Filter filter)
    {
        var response = await _HttpClient.DeleteAsync($"/filters/{filter.Name}");
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

    public async Task<LogsResponse> Execute(Filter filter)
    {
        try
        {
            var query = filter.Query;
            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = _NavigationManager.ToAbsoluteUri("/api")
            };
            var graphQLClient = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer());

            var request = new GraphQLRequest
            {
                Query = query.Replace("\n", " ")
            };

            var graphQLResponse = await graphQLClient
                .SendQueryAsync<FilterResponse>(request)
                .ConfigureAwait(false);

            return graphQLResponse.Data.Logs;
        }
        catch (Exception ex)
        {
            throw new ServiceException("Query cannot be resolved", ex);
        }
    }

    private class FilterResponse
    {
        public LogsResponse Logs { get; set; }
    }
}