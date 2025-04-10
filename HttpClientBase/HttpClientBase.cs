using System.Net.Http.Json;

namespace HttpClientBase;

public class HttpClientBase(HttpClient _httpClient) : IHttpClientBase {

    public async Task<TResponse?> DeleteAsync<TResponse>(string url)
    {
        var response = await _httpClient.DeleteAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>();
    }

    public async Task<TResponse?> GetAsync<TResponse>(string url)
    {
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>();
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest data)
    {
        var response = await _httpClient.PostAsJsonAsync(url, data);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>();
    }

    public async Task<TResponse?> PutAsync<TRequest, TResponse>(string url, TRequest data)
    {
        var response = await _httpClient.PutAsJsonAsync(url, data);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>();
    }

    public void SetAuthorizationHeader(string? token = null, string tokenType = "Bearer")
    {
        _httpClient.DefaultRequestHeaders.Authorization = string.IsNullOrEmpty(token) ? null :
                            new System.Net.Http.Headers.AuthenticationHeaderValue(tokenType, token);
    }
}