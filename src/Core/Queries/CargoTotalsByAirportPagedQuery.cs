namespace Core.Queries;

public sealed class CargoTotalsByAirportPagedQuery : GlobalFiltersDto, IRequest<PaginationResult<CargoTotalsByAirportResult>>, IPaginationFilterDto, ICacheableQuery
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string OrderBy { get; set; }
    public bool OrderByDescending { get; set; } = false;
    public bool UseSql { get; set; } = false;
    public bool BypassCache => false;
    public string CacheKey => GenerateCacheKey();
    public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(CacheConstants.DefaultTTL);

    public string GenerateCacheKey()
    {
        var result = $"{GetCacheKey(CacheConstants.CargoTotalsByAirportPagedQueryKey)}";

        result = string.Concat(result, ".UseSql:", UseSql);
        result = string.Concat(result, ".Page:", Page);
        result = string.Concat(result, ".PageSize:", PageSize);
        result = string.Concat(result, ".OrderBy:", OrderBy);
        result = string.Concat(result, ".OrderByDescending", OrderByDescending);

        return result;
    }

    public CargoTotalsByAirportPagedQuery(
        GlobalFiltersDto globalFiltersDto,
        PaginationFilterDto paginationFilterDto,
        bool useSql = false) : base(globalFiltersDto)
    {
        Page = paginationFilterDto.Page;
        PageSize = paginationFilterDto.PageSize;
        OrderBy = paginationFilterDto.OrderBy;
        OrderByDescending = paginationFilterDto.OrderByDescending;
        UseSql = useSql;
    }
}

public sealed class CargoTotalsByAirportPagedQueryHandler : IRequestHandler<CargoTotalsByAirportPagedQuery, PaginationResult<CargoTotalsByAirportResult>>
{
    private readonly IMediator _mediator;
    private readonly IAirportMongoRepository _airportMongoRepository;
    private readonly IAirportSqlRepository _airportSqlRepository;

    public CargoTotalsByAirportPagedQueryHandler(
        IMediator mediator,
        IAirportMongoRepository airportMongoRepository,
        IAirportSqlRepository airportSqlRepository)
    {
        _mediator = mediator;
        _airportMongoRepository = airportMongoRepository;
        _airportSqlRepository = airportSqlRepository;
    }

    public async Task<PaginationResult<CargoTotalsByAirportResult>> Handle(CargoTotalsByAirportPagedQuery request, CancellationToken cancellationToken)
    {
        var query = new CargoQuery(request);

        var cargos = await _mediator.Send(query, cancellationToken).ConfigureAwait(false);

        var airports = request.UseSql
            ? await _airportSqlRepository.GetAllItemsCachedAsync()
            : await _airportMongoRepository.GetAllItemsCachedAsync();

        return CargoTotalsByAirportResult.Result(request, cargos, airports);
    }
}

public sealed class CargoTotalsByAirportResult
{
    #region Properties
    public PerformanceEnum PerformanceStatus { get; set; }
    public decimal PerformancePercentage { get; set; }
    public string AirportCode { get; set; }
    public string AirportCityStateRegion { get; set; }
    public string AirportAddress { get; set; }
    public int QuantityItemsInFirstMile { get; set; }
    public int QuantityItemsInMiddleMile { get; set; }
    public int QuantityItemsInFloor { get; set; }
    public int QuantityItemsInLastMile { get; set; }
    public int QuantityItemsDelivered { get; set; }
    public int QuantityItemsTotal { get; set; } 
    #endregion

    public static PaginationResult<CargoTotalsByAirportResult> Result(CargoTotalsByAirportPagedQuery query, List<Cargo> cargos, List<Airport> airports)
    {
        var cargosGrouped = CargoTotalsByAirportsGrouped(cargos, airports);

        if (!string.IsNullOrEmpty(query.Performances))
        {
            var expressionFilter = PredicateBuilder.New<CargoTotalsByAirportResult>(true);
            expressionFilter = expressionFilter.And(item => query.PerformanceEnums.Contains(item.PerformanceStatus));

            cargosGrouped = cargosGrouped.Where(expressionFilter).ToList();
        }

        return cargosGrouped.OrderByPropertieName(query.OrderBy, query.OrderByDescending).GetPaged(query.Page, query.PageSize);
    }

    public static List<CargoTotalsByAirportResult> CargoTotalsByAirportsGrouped(List<Cargo> cargos, List<Airport> airports)
    {
        var cargoTotalsByAirportsGrouped = new List<CargoTotalsByAirportResult>(cargos.Count);

        cargoTotalsByAirportsGrouped.AddRange
            (from a in airports
             join c in cargos on a.AirportCode equals c.AirportIsOn
             into batch
             where batch.Any()
             select new CargoTotalsByAirportResult
             {
                 AirportCode = a.AirportCode,
                 AirportCityStateRegion = $"{a.City} - {Util.AbbreviatedState(a.State)} - {a.Region.GetDescription()}",
                 AirportAddress = a.Address,
                 QuantityItemsInFirstMile = batch.Count(c => c.StepFirstMile()),
                 QuantityItemsInMiddleMile = batch.Count(c => c.StepMiddleMile()),
                 QuantityItemsInFloor = batch.Count(c => c.ViewFloor()),
                 QuantityItemsInLastMile = batch.Count(c => c.StepLastMile()),
                 QuantityItemsDelivered = batch.Count(c => c.StepDelivered()),
                 QuantityItemsTotal = batch.Count(),
             });

        foreach (var item in cargoTotalsByAirportsGrouped)
        {
            var percentage = Util.CalculatePerformancePercentage(item.QuantityItemsTotal, item.QuantityItemsDelivered);
            item.PerformancePercentage = percentage;
            item.PerformanceStatus = Util.PerformanceByPercentage(percentage);
        }

        return cargoTotalsByAirportsGrouped;
    }
}
