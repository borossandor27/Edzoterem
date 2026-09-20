using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Edzoterem.Desktop.Services;

public class ApiClient
{
    private readonly HttpClient _httpClient;
    private readonly SessionState _session;
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public ApiClient(HttpClient httpClient, SessionState session)
    {
        _httpClient = httpClient;
        _session = session;
    }

    private void AlkalmazAuthFejlecet()
    {
        _httpClient.DefaultRequestHeaders.Authorization = _session.Token is not null
            ? new AuthenticationHeaderValue("Bearer", _session.Token)
            : null;
    }

    private static async Task EllenorizValaszAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var uzenet = "Ismeretlen hiba történt.";
        try
        {
            var body = await response.Content.ReadFromJsonAsync<JsonElement>();
            if (body.TryGetProperty("uzenet", out var uzenetProp))
            {
                uzenet = uzenetProp.GetString() ?? uzenet;
            }
        }
        catch
        {
            // a válasz nem JSON formátumú volt - marad az alapértelmezett üzenet
        }

        throw new ApiException(uzenet, (int)response.StatusCode);
    }

    public async Task<T?> GetAsync<T>(string relativeUrl)
    {
        AlkalmazAuthFejlecet();
        var response = await _httpClient.GetAsync(relativeUrl);
        await EllenorizValaszAsync(response);
        return await response.Content.ReadFromJsonAsync<T>(JsonOptions);
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string relativeUrl, TRequest body)
    {
        AlkalmazAuthFejlecet();
        var response = await _httpClient.PostAsJsonAsync(relativeUrl, body, JsonOptions);
        await EllenorizValaszAsync(response);
        return await response.Content.ReadFromJsonAsync<TResponse>(JsonOptions);
    }

    public async Task PostAsync<TRequest>(string relativeUrl, TRequest body)
    {
        AlkalmazAuthFejlecet();
        var response = await _httpClient.PostAsJsonAsync(relativeUrl, body, JsonOptions);
        await EllenorizValaszAsync(response);
    }

    public async Task<TResponse?> PutAsync<TRequest, TResponse>(string relativeUrl, TRequest body)
    {
        AlkalmazAuthFejlecet();
        var response = await _httpClient.PutAsJsonAsync(relativeUrl, body, JsonOptions);
        await EllenorizValaszAsync(response);
        return await response.Content.ReadFromJsonAsync<TResponse>(JsonOptions);
    }

    public async Task PutAsync(string relativeUrl)
    {
        AlkalmazAuthFejlecet();
        var response = await _httpClient.PutAsync(relativeUrl, content: null);
        await EllenorizValaszAsync(response);
    }

    public async Task DeleteAsync(string relativeUrl)
    {
        AlkalmazAuthFejlecet();
        var response = await _httpClient.DeleteAsync(relativeUrl);
        await EllenorizValaszAsync(response);
    }
}
