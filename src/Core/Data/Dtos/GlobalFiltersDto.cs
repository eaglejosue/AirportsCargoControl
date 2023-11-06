namespace Core.Data.Dtos;

public class GlobalFiltersDto
{
    #region Properties
    [Required] public DateTime RangeDateInitial { get; set; }
    [Required] public DateTime RangeDateFinal { get; set; }
    public string Performances { get; set; }
    [JsonIgnore] public List<PerformanceEnum> PerformanceEnums => Performances.GetListOfEnum<PerformanceEnum>();
    public string BrazilianStates { get; set; }
    [JsonIgnore] public List<AbbreviatedStatesEnum> BrazilianStateEnums => BrazilianStates.GetListOfEnum<AbbreviatedStatesEnum>();
    public string BrazilianRegions { get; set; }
    [JsonIgnore] public List<BrazilianRegionsEnum> BrazilianRegionEnums => BrazilianRegions.GetListOfEnum<BrazilianRegionsEnum>();
    public string Client { get; set; }
    [JsonIgnore] public List<string> ClientList => Client.GetListOfString();
    public string Actions { get; set; }
    [JsonIgnore] public List<string> ActionsList => Actions.GetListOfString();
    public string Origin { get; set; }
    [JsonIgnore] public List<string> OriginList => Origin.GetListOfString();
    public string Destination { get; set; }
    [JsonIgnore] public List<string> DestinationList => Destination.GetListOfString();
    public string ActualLocation { get; set; }
    [JsonIgnore] public List<string> ActualLocationList => ActualLocation.GetListOfString();
    #endregion

    #region Constructors
    public GlobalFiltersDto() { }
    public GlobalFiltersDto(GlobalFiltersDto dto = null)
    {
        RangeDateInitial = dto?.RangeDateInitial ?? DateTime.Today;
        RangeDateFinal = dto?.RangeDateFinal ?? DateTime.Today;
        Performances = dto?.Performances;
        BrazilianStates = dto?.BrazilianStates;
        BrazilianRegions = dto?.BrazilianRegions;
        Client = dto?.Client;
        Actions = dto?.Actions;
        Origin = dto?.Origin;
        Destination = dto?.Destination;
        ActualLocation = dto?.ActualLocation;
    }
    #endregion

    #region Methods

    public void PerformanceFilter(ref List<Cargo> cargos)
    {
        if (!string.IsNullOrEmpty(Performances))
        {
            cargos = cargos.Where(item => PerformanceEnums.Contains(item.Performance)).ToList();
            cargos.Capacity = cargos.Count();
        }
    }

    public void ClearPerformances()
    {
        if (!string.IsNullOrEmpty(Performances))
            Performances = string.Empty;
    }

    public string GetCacheKey(string key) => $"{key}:{ToString()}";

    public string ToString(string c = ".")
    {
        GetConcatenedProperties(
            out var performances,
            out var brazilianStates,
            out var brazilianRegions,
            out var client,
            out var action,
            out var origin,
            out var destination,
            out var actualLocation);

        var result = string.Concat(RangeDateInitial.ToString("dd-MM-yyyy"), c, RangeDateFinal.ToString("dd-MM-yyyy"));

        if (!string.IsNullOrEmpty(performances)) result = string.Concat(result, c, "Performance-", performances);
        if (!string.IsNullOrEmpty(brazilianStates)) result = string.Concat(result, c, "Stado-", brazilianStates);
        if (!string.IsNullOrEmpty(brazilianRegions)) result = string.Concat(result, c, "Regiao-", brazilianRegions);
        if (!string.IsNullOrEmpty(client)) result = string.Concat(result, c, "Cliente-", client);
        if (!string.IsNullOrEmpty(action)) result = string.Concat(result, c, "Action-", action);
        if (!string.IsNullOrEmpty(origin)) result = string.Concat(result, c, "Origem-", origin);
        if (!string.IsNullOrEmpty(destination)) result = string.Concat(result, c, "Destino-", destination);
        if (!string.IsNullOrEmpty(actualLocation)) result = string.Concat(result, c, "BaseAtual-", actualLocation);

        return result;
    }

    private void GetConcatenedProperties(
        out string performances,
        out string brazilianStates,
        out string brazilianRegions,
        out string client,
        out string action,
        out string origin,
        out string destination,
        out string actualLocation)
    {
        performances = string.Join("-", Performances ?? "");
        brazilianStates = string.Join("-", BrazilianStates ?? "");
        brazilianRegions = string.Join("-", BrazilianRegions ?? "");
        client = string.Join("-", Client ?? "");
        action = string.Join("-", Actions ?? "");
        origin = string.Join("-", Origin ?? "");
        destination = string.Join("-", Destination ?? "");
        actualLocation = string.Join("-", ActualLocation ?? "");
    }

    #endregion
}
