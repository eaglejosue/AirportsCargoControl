namespace Core.Cache;

public sealed class RedisCache : ICache
{
    private readonly ConnectionMultiplexer _connection;
    private readonly IDatabase _database;

    public RedisCache(
        ConnectionMultiplexer connection,
        int database = 1
    )
    {
        _connection = connection;
        _database = _connection.GetDatabase(database);
    }

    public Task AddAsync<T>(string key, T obj, int expirationMinutes) =>
        AddAsync(key, obj, TimeSpan.FromMinutes(expirationMinutes));

    public Task AddAsync<T>(string key, T? obj, TimeSpan expirationDuration)
    {
        if (obj is null) return Task.CompletedTask;
        byte[] array = obj.ToJsonBytes();
        return _database.StringSetAsync(key, array, expirationDuration);
    }

    public Task<bool> ExistsAsync(string key) => _database.KeyExistsAsync(key);

    public async Task<IEnumerable<string>> GetAllKeysAsync(string? pattern = null)
    {
        var returnKeys = new List<string>();
        await Task.Run(delegate
        {
            var endPoints = _connection.GetEndPoints(configuredOnly: true);
            foreach (var endpoint in endPoints)
            {
                var server = _connection.GetServer(endpoint);
                returnKeys.AddRange((pattern == null)
                    ? (from key in server.Keys(0, default, 10, 0L) select key.ToString())
                    : (from key in server.Keys(0, pattern, 10, 0L) select key.ToString()));
            }
        });
        return returnKeys;
    }

    public async Task RemoveAllAsync()
    {
        var endPoints = _connection.GetEndPoints(true);
        foreach (var endpoint in endPoints)
            await _connection.GetServer(endpoint).FlushAllDatabasesAsync();
    }

    public Task RemoveAsync(string key) => _database.KeyDeleteAsync(key);

    public async Task<T?> RetrieveAsync<T>(string key)
    {
        RedisValue redisValue = await _database.StringGetAsync(key);
        return redisValue.HasValue ? ((byte[])redisValue).ParseJson<T>() : default;
    }

    public Task<T> RetrieveAsync<T>(string key, int expirationMinutes) =>
        RetrieveAsync<T>(key, TimeSpan.FromMinutes(expirationMinutes));

    public async Task<T> RetrieveAsync<T>(string key, TimeSpan expirationDuration)
    {
        T obj = await RetrieveAsync<T>(key);
        if (!Equals(obj, default(T)))
            await _database.KeyExpireAsync(key, expirationDuration);
        
        return obj;
    }

    public Task<T> RetrieveOrAddAsync<T>(string key, Func<T> action, int expirationMinutes) =>
        RetrieveOrAddAsync(key, action, TimeSpan.FromMinutes(expirationMinutes));

    public async Task<T> RetrieveOrAddAsync<T>(string key, Func<T> action, TimeSpan expirationDuration)
    {
        RedisValue redisValue = await _database.StringGetAsync(key);
        if (redisValue.HasValue)
            return ((byte[])redisValue).ParseJson<T>();

        T obj = action();
        await AddAsync(key, obj, expirationDuration);
        return obj;
    }
}
