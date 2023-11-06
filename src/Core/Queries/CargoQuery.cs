namespace Core.Queries;

public sealed class CargoQuery : GlobalFiltersDto, IRequest<List<Cargo>>, ICacheableQuery
{
    public bool UseSql { get; set; } = false;
    public bool BypassCache => false;
    public string CacheKey => GenerateCacheKey();
    public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(CacheConstants.DefaultTTL);

    public string GenerateCacheKey() => $"{GetCacheKey(CacheConstants.CargoQueryKey)}.UseSql:{UseSql}";

    public CargoQuery(
        GlobalFiltersDto globalFiltersDto,
        bool useSql = false) : base(globalFiltersDto)
    {
        UseSql = useSql;
    }
}

public sealed class CargoQueryHandler : IRequestHandler<CargoQuery, List<Cargo>>
{
    private readonly ICargoMongoRepository _cargoMongoRepository;
    private readonly ICargoSqlRepository _cargoSqlRepository;

    public CargoQueryHandler(
        ICargoMongoRepository cargoMongoRepository,
        ICargoSqlRepository cargoSqlRepository)
    {
        _cargoMongoRepository = cargoMongoRepository;
        _cargoSqlRepository = cargoSqlRepository;
    }

    public async Task<List<Cargo>> Handle(CargoQuery request, CancellationToken cancellationToken)
    {
        var filter = request.GenerateFilterExpression(true);

        var cargos = request.UseSql
            ? await _cargoSqlRepository.SelectAsync(filter)
            : await _cargoMongoRepository.FilterByAsync(filter, c => new Cargo());

        CargoExtensions.SetPerformance(ref cargos);

        request.PerformanceFilter(ref cargos);

        return cargos;
    }
}
