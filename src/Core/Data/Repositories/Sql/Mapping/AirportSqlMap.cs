namespace Core.Data.Repositories.Sql.Mapping;

public sealed class AirportSqlMap : DommelEntityMap<Airport>
{
    public AirportSqlMap()
    {
        ToTable(nameof(Airport));
        Map(m => m.Id).IsKey().SetGeneratedOption(DatabaseGeneratedOption.None);
        Map(m => m.AirportCode);
        Map(m => m.Region);
        Map(m => m.State);
        Map(m => m.City);
        Map(m => m.Lat);
        Map(m => m.Long);
        Map(m => m.Address);
    }
}
