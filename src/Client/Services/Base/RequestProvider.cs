using Baustellen.App.Client.Exceptions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Baustellen.App.Client.Services.Base;

public class RequestProvider(HttpMessageHandler _messageHandler)
{
    private readonly Lazy<HttpClient> _httpClient =
        new(() =>
        {
            var httpClient = _messageHandler is not null ? new HttpClient(_messageHandler) : new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return httpClient;
        },
            LazyThreadSafetyMode.ExecutionAndPublication);

    public async Task<TResult?> GetAsync<TResult>(string uri, string token = "")
    {
        var httpClient = GetOrCreateHttpClient(token);
        using var response = await httpClient.GetAsync(uri).ConfigureAwait(false);

        await HandleResponse(response).ConfigureAwait(false);

        var result = await ReadFromJsonAsync<TResult>(response.Content).ConfigureAwait(false);

        return result;
    }

    public async Task<string> GetAsync(string uri, string token = "")
    {
        var httpClient = GetOrCreateHttpClient(token);
        using var response = await httpClient.GetAsync(uri).ConfigureAwait(false);

        await HandleResponse(response).ConfigureAwait(false);

        var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        return result;
    }

    public async Task<TResult?> PostAsync<TRequest, TResult>(string uri, TRequest data, string token = "", string header = "")
    {
        var httpClient = GetOrCreateHttpClient(token);

        if (!string.IsNullOrEmpty(header))
        {
            AddHeaderParameter(httpClient, header);
        }

        var requestContent = SerializeToJson(data);
        using HttpResponseMessage response = await httpClient.PostAsync(uri, requestContent).ConfigureAwait(false);

        await HandleResponse(response).ConfigureAwait(false);
        var result = await ReadFromJsonAsync<TResult>(response.Content).ConfigureAwait(false);

        return result;
    }

    public async Task<bool> PostAsync<TRequest>(string uri, TRequest data, string token = "", string header = "")
    {
        var httpClient = GetOrCreateHttpClient(token);

        if (!string.IsNullOrEmpty(header))
        {
            AddHeaderParameter(httpClient, header);
        }

        var requestContent = SerializeToJson(data);
        using var response = await httpClient.PostAsync(uri, requestContent).ConfigureAwait(false);

        await HandleResponse(response).ConfigureAwait(false);

        return response.IsSuccessStatusCode;
    }

    public async Task<TResult?> PutAsync<TResult>(string uri, TResult data, string token = "", string header = "")
    {
        var httpClient = GetOrCreateHttpClient(token);

        if (!string.IsNullOrEmpty(header))
        {
            AddHeaderParameter(httpClient, header);
        }

        var requestContent = SerializeToJson(data);
        using HttpResponseMessage response = await httpClient.PutAsync(uri, requestContent).ConfigureAwait(false);

        await HandleResponse(response).ConfigureAwait(false);
        var result = await ReadFromJsonAsync<TResult>(response.Content).ConfigureAwait(false);

        return result;
    }

    public async Task DeleteAsync(string uri, string token = "")
    {
        var httpClient = GetOrCreateHttpClient(token);
        await httpClient.DeleteAsync(uri).ConfigureAwait(false);
    }

    private HttpClient GetOrCreateHttpClient(string token = "")
    {
        var httpClient = _httpClient.Value;

        httpClient.DefaultRequestHeaders.Authorization =
            !string.IsNullOrEmpty(token)
                ? new AuthenticationHeaderValue("Bearer", token)
                : null;

        return httpClient;
    }

    private static void AddHeaderParameter(HttpClient httpClient, string parameter)
    {
        if (httpClient == null)
        {
            return;
        }

        if (string.IsNullOrEmpty(parameter))
        {
            return;
        }

        httpClient.DefaultRequestHeaders.Add(parameter, Guid.NewGuid().ToString());
    }

    private static async Task HandleResponse(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.Forbidden || response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new ServiceAuthenticationException(content);
            }

            throw new HttpRequestExceptionEx(response.StatusCode, content);
        }
    }

    private static async Task<T?> ReadFromJsonAsync<T>(HttpContent content)
    {
        using var contentStream = await content.ReadAsStreamAsync().ConfigureAwait(false);
        var data = await JsonSerializer.DeserializeAsync(contentStream, typeof(T), BaustellenAppSerialziationContext.Default).ConfigureAwait(false);
        return (T?)data;
    }

    private static JsonContent SerializeToJson<T>(T data)
    {
        var typeInfo = BaustellenAppSerialziationContext.Default.GetTypeInfo(typeof(T)) ?? throw new InvalidOperationException($"Missing type info for {typeof(T)}");
        return JsonContent.Create(data, typeInfo);
    }
}
