namespace Core.Utils;

public static class Util
{
    public static List<AbbreviatedStatesEnum> StatesByRegion(List<BrazilianRegionsEnum> listRegions)
    {
        var returnItems = new List<AbbreviatedStatesEnum>();

        if (listRegions == null || !listRegions.Any())
            return returnItems;

        foreach (var region in listRegions)
        {
            switch (region)
            {
                case BrazilianRegionsEnum.North:
                    returnItems.AddRange(new List<AbbreviatedStatesEnum>
                        {
                            AbbreviatedStatesEnum.AC,
                            AbbreviatedStatesEnum.AM,
                            AbbreviatedStatesEnum.PA,
                            AbbreviatedStatesEnum.RO,
                            AbbreviatedStatesEnum.RR,
                            AbbreviatedStatesEnum.TO
                        });
                    break;

                case BrazilianRegionsEnum.NorthEast:
                    returnItems.AddRange(new List<AbbreviatedStatesEnum>
                        {
                            AbbreviatedStatesEnum.AL,
                            AbbreviatedStatesEnum.BA,
                            AbbreviatedStatesEnum.CE,
                            AbbreviatedStatesEnum.MA,
                            AbbreviatedStatesEnum.PR,
                            AbbreviatedStatesEnum.PE,
                            AbbreviatedStatesEnum.PI,
                            AbbreviatedStatesEnum.RN,
                            AbbreviatedStatesEnum.SE,
                        });
                    break;

                case BrazilianRegionsEnum.Midwest:
                    returnItems.AddRange(new List<AbbreviatedStatesEnum>
                        {
                            AbbreviatedStatesEnum.GO,
                            AbbreviatedStatesEnum.DF,
                            AbbreviatedStatesEnum.MT,
                            AbbreviatedStatesEnum.MS,
                        });
                    break;

                case BrazilianRegionsEnum.Southeast:
                    returnItems.AddRange(new List<AbbreviatedStatesEnum>
                        {
                            AbbreviatedStatesEnum.SP,
                            AbbreviatedStatesEnum.ES,
                            AbbreviatedStatesEnum.MG,
                            AbbreviatedStatesEnum.RJ,
                        });
                    break;

                case BrazilianRegionsEnum.South:
                    returnItems.AddRange(new List<AbbreviatedStatesEnum>
                        {
                            AbbreviatedStatesEnum.PR,
                            AbbreviatedStatesEnum.SC,
                            AbbreviatedStatesEnum.RS,
                        });
                    break;
            }
        }

        return returnItems.Distinct().ToList();
    }

    public static string AbbreviatedState(BrazilianStatesEnum? state)
    {
        if (!state.HasValue)
            return "XX";

        var abbreviatedStates = GeneralConstants.AbbreviatedStates;

        return abbreviatedStates.FirstOrDefault(x => x.Key == state).Value;
    }

    public static BrazilianStatesEnum GetStateByAbbreviation(AbbreviatedStatesEnum state)
    {
        var abbreviatedStates = GeneralConstants.AbbreviatedStates;
        return abbreviatedStates.FirstOrDefault(x => x.Value == state.ToString()).Key;
    }

    public static List<BrazilianStatesEnum> GetStatesByAbbreviations(List<AbbreviatedStatesEnum> state)
    {
        var abbreviatedStates = GeneralConstants.AbbreviatedStates;
        var statesString = state.Select(x => x.ToString());

        return abbreviatedStates.Where(x => statesString.Contains(x.Value)).Select(x => x.Key).ToList();
    }

    public static decimal TruncatePercentage(this decimal value) => Convert.ToDecimal((value / 100).ToString("P2").Replace("%", ""));

    public static PerformanceEnum PerformanceByPercentage(decimal percentage)
    {
        if (percentage >= 98)
            return PerformanceEnum.Normal;

        if (percentage >= 94 && percentage <= 97)
            return PerformanceEnum.AtRisk;

        return PerformanceEnum.Late;
    }

    public static decimal CalculatePerformancePercentage(IEnumerable<PerformanceEnum> performances)
    {
        var quantityTotal = performances.Count();
        var quantityNormal = performances.Count(item => item == PerformanceEnum.Normal);

        return CalculatePerformancePercentage(quantityTotal, quantityNormal);
    }

    public static decimal CalculatePerformancePercentage(int quantityTotal, int quantityNormal)
    {
        if (quantityTotal == 0)
            return 100M;

        if (quantityNormal == 0)
            return 0M;

        var percentage = 100 * Convert.ToDecimal(quantityNormal) / quantityTotal;
        var percentageFormatted = percentage.TruncatePercentage();

        return percentageFormatted;
    }
}
