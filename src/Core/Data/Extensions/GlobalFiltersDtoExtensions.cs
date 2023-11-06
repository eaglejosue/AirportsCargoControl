namespace Core.Data.Extensions;

public static class GlobalFiltersDtoExtensions
{
    public static Expression<Func<Cargo, bool>> GenerateFilterExpression(
            this GlobalFiltersDto filtersDTO,
            bool useOrForOriginDestinationAndLastOpsStation = false,
            bool useForUnitTest = false)
    {
        var filter = PredicateBuilder.New<Cargo>(true);

        filtersDTO.RangeDateInitial = filtersDTO.RangeDateInitial.DateTimeUtc(true);
        filtersDTO.RangeDateFinal = filtersDTO.RangeDateFinal.DateTimeUtc(false);
        filter = filter.And(item => item.EstimatedDeliveryDate >= filtersDTO.RangeDateInitial && item.EstimatedDeliveryDate <= filtersDTO.RangeDateFinal);

        if (filtersDTO.BrazilianRegionEnums.Count > 0)
        {
            var brazilianStates = Util.StatesByRegion(filtersDTO.BrazilianRegionEnums);
            filtersDTO.BrazilianStates = filtersDTO.BrazilianStates.ConcatenateWithComma(string.Join(',', brazilianStates));
        }

        ClientFilter(filtersDTO, ref filter, useForUnitTest);

        if (useForUnitTest)
        {
            if (filtersDTO.BrazilianStateEnums.Count > 0)
                filter = filter.And(item => filtersDTO.BrazilianStateEnums.Contains(item.StateIsOn));
            if (filtersDTO.ActionsList.Count > 0)
                filter = filter.And(item => filtersDTO.ActionsList.Contains(item.LastAction));
            if (filtersDTO.OriginList.Count > 0)
                filter = filter.And(item => filtersDTO.OriginList.Contains(item.Origin));
            if (filtersDTO.DestinationList.Count > 0)
                filter = filter.And(item => filtersDTO.DestinationList.Contains(item.Destination));
            if (filtersDTO.ActualLocationList.Count > 0)
                filter = filter.And(item => filtersDTO.ActualLocationList.Contains(item.AirportIsOn));
        }
        else
        {
            CreateFilterWithList(ref filter, filtersDTO.BrazilianStateEnums, item => item.StateIsOn);
            CreateFilterWithList(ref filter, filtersDTO.ActionsList, item => item.LastAction);

            //Regra para filtrar cs com Origem OU Destino OU Localização Atual
            if (useOrForOriginDestinationAndLastOpsStation)
            {
                var originsFilter = Builders<Cargo>.Filter.In(item => item.Origin, filtersDTO.OriginList);
                var destinationsFilter = Builders<Cargo>.Filter.In(item => item.Destination, filtersDTO.DestinationList);
                var lastOpsStationsFilter = Builders<Cargo>.Filter.In(item => item.AirportIsOn, filtersDTO.ActualLocationList);

                filter = filter.And(item => originsFilter.Inject() || destinationsFilter.Inject() || lastOpsStationsFilter.Inject());
            }

            CreateFilterWithList(ref filter, filtersDTO.OriginList, item => item.Origin);
            CreateFilterWithList(ref filter, filtersDTO.DestinationList, item => item.Destination);
            CreateFilterWithList(ref filter, filtersDTO.ActualLocationList, item => item.AirportIsOn);
        }

        return filter;
    }

    private static void ClientFilter(this GlobalFiltersDto filtersDTO, ref ExpressionStarter<Cargo> filter, bool useForUnitTest = false)
    {
        var client = filtersDTO.ClientList.Select(s => s.RemoveCharacters(new char[3] { '.', '/', '-' }));
        var clientNames = client.Where(w => char.IsLetter(w[0])).ToList();
        var clientCodes = client.Where(w => char.IsDigit(w[0])).ToList();

        if (useForUnitTest)
        {
            if (clientNames.Count > 0 && clientCodes.Count > 0)
            {
                filter = filter.And(item => clientNames.Contains(item.ClientName) || clientCodes.Contains(item.ClientCode));
                return;
            }

            if (clientNames.Count > 0)
                filter = filter.And(item => clientNames.Contains(item.ClientName));

            if (clientCodes.Count > 0)
                filter = filter.And(item => clientCodes.Contains(item.ClientCode));
        }
        else
        {
            if (clientNames.Count > 0 && clientCodes.Count > 0)
            {
                var filterCustomerNames = Builders<Cargo>.Filter.In(item => item.ClientName, clientNames);
                var filterCustomerCodes = Builders<Cargo>.Filter.In(item => item.ClientCode, clientCodes);
                filter = filter.And(item => filterCustomerNames.Inject() || filterCustomerCodes.Inject());
                return;
            }

            CreateFilterWithList(ref filter, clientNames, item => item.ClientName);
            CreateFilterWithList(ref filter, clientCodes, item => item.ClientCode);
        }
    }

    private static void CreateFilterWithList<T>(ref ExpressionStarter<Cargo> filter, IEnumerable<T> values, Expression<Func<Cargo, T>> field)
    {
        if (values?.Count() == 0)
            return;

        var filterBuilder = Builders<Cargo>.Filter.In(field, values);
        filter = filter.And(item => filterBuilder.Inject());
    }
}
