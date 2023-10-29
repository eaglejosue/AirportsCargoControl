namespace Core.Data.Repositories.Sql.Mapping;

public sealed class CargoSqlMap : DommelEntityMap<Cargo>
{
    public CargoSqlMap()
    {
        ToTable(nameof(Cargo));
        Map(m => m.Id).IsKey().SetGeneratedOption(DatabaseGeneratedOption.None);
        Map(m => m.AirWaybill);
        Map(m => m.EstimatedDeliveryDate);
        Map(m => m.CreatedOn);
        Map(m => m.LastUpdatedOn);
        Map(m => m.LastOpsStation);
        Map(m => m.LastAction);
        Map(m => m.Origin);
        Map(m => m.Destination);
        Map(m => m.ActualStep);
    }
}
