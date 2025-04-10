namespace HttpClientBase;

public interface IHttpClientBase {

    /// <summary>
    /// Get data from API
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="url"></param>
    /// <returns></returns>
    Task<TResponse?> GetAsync<TResponse>(string url);

    /// <summary>
    /// Tạo mới dữ liệu
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="url"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest data);

    /// <summary>
    /// Cập nhật dữ liệu, hoặc Post dữ liệu lên server và nhận về dữ liệu
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="url"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    Task<TResponse?> PutAsync<TRequest, TResponse>(string url, TRequest data);

    /// <summary>
    /// Xóa dữ liệu
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="url"></param>
    /// <returns></returns>
    Task<TResponse?> DeleteAsync<TResponse>(string url);

    void SetAuthorizationHeader(string? token = null, string tokenType = "Bearer");
}