namespace Core.Utils;

public static class CargoStepsContants
{
    public const string FirstMile = "First Mile";
    public const string LastMile = "Last Mile";
    public const string MiddleMile = "Middle Mile";
    public const string Delivered = "Delivered";
}

public static class CacheConstants
{
    public const int DefaultTTL = 5;//min
    public const string CargoByAirportPagedQueryKey = "CargoByAirportPagedQuery";
}

public static class DataCacheContants
{
    public const string AirportRepository = "AirportRepository";
    public const int AirportRepositoryTTL = 24;//hr
}

public static class GeneralConstants
{
    public static ImmutableDictionary<BrazilianStatesEnum, string> AbbreviatedStates = new Dictionary<BrazilianStatesEnum, string>()
    {
        {BrazilianStatesEnum.None, ""},
        {BrazilianStatesEnum.DistritoFederal, "DF"},
        {BrazilianStatesEnum.RioGrandeDoSul, "RS"},
        {BrazilianStatesEnum.SantaCatarina, "SC"},
        {BrazilianStatesEnum.Parana, "PR"},
        {BrazilianStatesEnum.SaoPaulo, "SP"},
        {BrazilianStatesEnum.RioDeJaneiro, "RJ"},
        {BrazilianStatesEnum.MinasGerais, "MG"},
        {BrazilianStatesEnum.EspiritoSanto, "ES"},
        {BrazilianStatesEnum.MatoGrossoDoSul, "MS"},
        {BrazilianStatesEnum.MatoGrosso, "MT"},
        {BrazilianStatesEnum.Goias, "GO"},
        {BrazilianStatesEnum.Sergipe, "SE"},
        {BrazilianStatesEnum.RioGrandeDoNorte, "RN"},
        {BrazilianStatesEnum.Piaui, "PI"},
        {BrazilianStatesEnum.Pernambuco, "PE"},
        {BrazilianStatesEnum.Paraiba, "PB"},
        {BrazilianStatesEnum.Maranhao, "MA"},
        {BrazilianStatesEnum.Ceara, "CE"},
        {BrazilianStatesEnum.Bahia, "BA"},
        {BrazilianStatesEnum.Alagoas, "AL"},
        {BrazilianStatesEnum.Tocantins, "TO"},
        {BrazilianStatesEnum.Roraima, "RR"},
        {BrazilianStatesEnum.Rondonia, "RO"},
        {BrazilianStatesEnum.Para, "PA"},
        {BrazilianStatesEnum.Amazonas, "AM"},
        {BrazilianStatesEnum.Amapa, "AP"},
        {BrazilianStatesEnum.Acre, "AC"},
    }.ToImmutableDictionary();
}
