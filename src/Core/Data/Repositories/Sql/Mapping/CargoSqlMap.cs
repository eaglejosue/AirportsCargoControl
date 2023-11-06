namespace Core.Data.Repositories.Sql.Mapping;

public sealed class CargoSqlMap : DommelEntityMap<Cargo>
{
    public CargoSqlMap()
    {
        ToTable(nameof(Cargo));
        Map(m => m.Id).IsKey().SetGeneratedOption(DatabaseGeneratedOption.None);
        Map(m => m.Code);
        Map(m => m.EstimatedDeliveryDate);
        Map(m => m.CreatedOn);
        Map(m => m.UpdatedOn);
        Map(m => m.AirportIsOn);
        Map(m => m.LastAction);
        Map(m => m.Origin);
        Map(m => m.Destination);
        Map(m => m.Step);
    }
}
