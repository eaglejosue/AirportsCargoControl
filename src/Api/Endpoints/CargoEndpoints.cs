namespace Api.Endpoints;

public static class CargoEndpoints
{
    public static void ConfigureCargoEndpoints(this WebApplication app)
    {
        app.MapGet("api/cargo/totals-by-airport",
        async (
            [FromQuery] CargoTotalsByAirportPagedQuery query,
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var paginationResult = await mediator.Send(query, cancellationToken).ConfigureAwait(false);
            
            if (!paginationResult?.Results?.Any() ?? false)
                return Results.NoContent();

            return Results.Ok(paginationResult);
        });

        app.MapPost("api/cargo/mongo-to-sql",
        async (
            DateTime RangeDateInitial,
            DateTime RangeDateFinal,
            [FromServices] ICargoMongoRepository cargoMongoRepository,
            [FromServices] ICargoSqlRepository cargoSqlRepository,
            CancellationToken cancellationToken) =>
        {
            var filter = PredicateBuilder.New<Cargo>(true)
                .And(item => item.EstimatedDeliveryDate >= RangeDateInitial)
                .And(item => item.EstimatedDeliveryDate <= RangeDateFinal);

            var cargos = await cargoMongoRepository.FilterByAsync(filter);
            
            await cargoSqlRepository.BulkInsertAsync(cargos);

            return Results.Ok($"{cargos.Count} items inserted on sql.");
        });
    }
}
