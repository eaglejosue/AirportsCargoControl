namespace Core.Cache;

public sealed class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : ICacheableQuery, IRequest<TResponse>
{
    private readonly ICache _cache;
    private readonly ILogger _logger;

    public CachingBehavior(ICache cache, ILogger<TResponse> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        TResponse response;

        if (request.BypassCache) return await next();

        async Task<TResponse> GetResponseAndAddToCacheAsync()
        {
            response = await next();
            await _cache.AddAsync(request.CacheKey, response, request?.SlidingExpiration ?? TimeSpan.FromMinutes(1));
            return response;
        }

        var cachedResponse = await _cache.RetrieveAsync<TResponse>(request.CacheKey);
        if (cachedResponse != null)
        {
            response = cachedResponse;
            _logger.LogInformation("Fetched from Cache -> 'CacheKey'.", request.CacheKey);
        }
        else
        {
            response = await GetResponseAndAddToCacheAsync();
            _logger.LogInformation("Added to Cache -> '{CacheKey}'.", request.CacheKey);
        }

        return response;
    }
}
