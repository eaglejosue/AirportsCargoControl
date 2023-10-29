namespace Core.Cache;

public interface ICache
{
    Task AddAsync<T>(string key, T obj, int expirationMinutes);
    Task AddAsync<T>(string key, T obj, TimeSpan expirationDuration);
    Task RemoveAsync(string key);
    Task RemoveAllAsync();
    Task<bool> ExistsAsync(string key);
    Task<IEnumerable<string>> GetAllKeysAsync(string? pattern = null);
    Task<T> RetrieveAsync<T>(string key);
    Task<T> RetrieveAsync<T>(string key, int expirationMinutes);
    Task<T> RetrieveAsync<T>(string key, TimeSpan expirationDuration);
    Task<T> RetrieveOrAddAsync<T>(string key, Func<T> action, int expirationMinutes);
    Task<T> RetrieveOrAddAsync<T>(string key, Func<T> action, TimeSpan expirationDuration);
}
