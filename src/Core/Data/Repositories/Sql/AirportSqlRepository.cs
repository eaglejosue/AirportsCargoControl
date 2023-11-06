namespace Core.Data.Repositories.Sql;

public interface IAirportSqlRepository : ISqlGenericRepository<Airport>
{
    Task<List<Airport>> GetAllItemsCachedAsync();
}

public sealed class AirportSqlRepository : SqlGenericRepository<Airport>, IAirportSqlRepository
{
    private readonly ICache _cache;

    public AirportSqlRepository(SqlSession sqlSession, ICache cache) : base(sqlSession)
    {
        _cache = cache;
    }

    public async Task<List<Airport>> GetAllItemsCachedAsync() => await _cache.RetrieveOrAddAsync(
        DataCacheContants.AirportRepository,
        () =>
        {
            return Task.Run(async () =>
            {
                return await SelectAsync(PredicateBuilder.New<Airport>());
            })
            .GetAwaiter()
            .GetResult();
        },
        DataCacheContants.AirportRepositoryTTL
    ).ConfigureAwait(false);
}
