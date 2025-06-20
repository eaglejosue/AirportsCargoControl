﻿namespace Core.Data.Repositories.Mongo;

public interface IAirportMongoRepository : IMongoGenericRepository<Airport, Guid>
{
    Task<List<Airport>> GetAllItemsCachedAsync();
}

public sealed class AirportMongoRepository : MongoGenericRepository<Airport, Guid>, IAirportMongoRepository
{
    private readonly ICache _cache;

    public AirportMongoRepository(
        IMongoDbSettings settings,
        ICache cache) : base(settings)
    {
        _cache = cache;
    }

    public async Task<List<Airport>> GetAllItemsCachedAsync() => await _cache.RetrieveOrAddAsync(
        DataCacheContants.AirportRepository,
        () =>
        {
            return Task.Run(async () =>
            {
                return await _collection.Find(Builders<Airport>.Filter.Empty).ToListAsync();
            })
            .GetAwaiter()
            .GetResult();
        },
        DataCacheContants.AirportRepositoryTTL
    ).ConfigureAwait(false);
}
