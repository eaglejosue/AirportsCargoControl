namespace Core.Queries;

public sealed class CargoByAirportPagedQuery : GlobalFiltersDto, IRequest<PaginationResult<Cargo>>, IPaginationFilterDto, ICacheableQuery
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string OrderBy { get; set; }
    public bool OrderByDescending { get; set; } = false;
    public string CodeAirport { get; set; }
    public bool UseSql { get; set; } = false;
    public bool BypassCache => false;
    public string CacheKey => GenerateCacheKey();
    public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(CacheConstants.DefaultTTL);

    public string GenerateCacheKey()
    {
        var result = $"{GetCacheKey(CacheConstants.CargoByAirportPagedQueryKey)}";

        if (!string.IsNullOrEmpty(CodeAirport))
            result = string.Concat(result, ".CodeAirport:", CodeAirport);

        result = string.Concat(result, ".UseSql:", UseSql);
        result = string.Concat(result, ".Page:", Page);
        result = string.Concat(result, ".PageSize:", PageSize);
        result = string.Concat(result, ".OrderBy:", OrderBy);
        result = string.Concat(result, ".OrderByDescending", OrderByDescending);

        return result;
    }

    public CargoByAirportPagedQuery() { }

    public CargoByAirportPagedQuery(
        GlobalFiltersDto globalFiltersDto,
        PaginationFilterDto paginationFilterDto,
        string codeAirport,
        bool useSql = false) : base(globalFiltersDto)
    {
        Page = paginationFilterDto.Page;
        PageSize = paginationFilterDto.PageSize;
        OrderBy = paginationFilterDto.OrderBy;
        OrderByDescending = paginationFilterDto.OrderByDescending;
        CodeAirport = codeAirport;
        UseSql = useSql;
    }
}

public sealed class CargoByAirportPagedQueryHandler : IRequestHandler<CargoByAirportPagedQuery, PaginationResult<Cargo>>
{
    private readonly ICargoMongoRepository _cargoMongoRepository;
    private readonly ICargoSqlRepository _cargoSqlRepository;

    public CargoByAirportPagedQueryHandler(
        ICargoMongoRepository cargoMongoRepository,
        ICargoSqlRepository cargoSqlRepository)
    {
        _cargoMongoRepository = cargoMongoRepository;
        _cargoSqlRepository = cargoSqlRepository;
    }

    public async Task<PaginationResult<Cargo>> Handle(CargoByAirportPagedQuery request, CancellationToken cancellationToken)
    {
        var filter = request.GenerateFilterExpression(true);

        var cargos = request.UseSql
            ? await _cargoSqlRepository.SelectAsync(filter)
            : await _cargoMongoRepository.FilterByAsync(filter,
            c => new Cargo
            {
                AirWaybill = c.AirWaybill,
                EstimatedDeliveryDate = c.EstimatedDeliveryDate,
                CreatedOn = c.CreatedOn,
                LastUpdatedOn = c.LastUpdatedOn,
                LastOpsStation = c.LastOpsStation,
                LastAction = c.LastAction,
                Origin = c.Origin,
                Destination = c.Destination,
                ActualStep = c.ActualStep,
                DeliveryDateTime = c.DeliveryDateTime,
                ServiceTakerCode = c.ServiceTakerCode,
                ServiceTakerName = c.ServiceTakerName,
                State = c.State
            });

        request.PerformanceFilter(ref cargos);

        return cargos.OrderByPropertieName(request.OrderBy, request.OrderByDescending).GetPaged(request.Page, request.PageSize);
    }
}
