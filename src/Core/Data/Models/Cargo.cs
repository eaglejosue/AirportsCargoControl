namespace Core.Data.Models;

[MongoCollection("Awb")]
public sealed class Cargo : Document<Guid>
{
    public string AirWaybill { get; set; }
    public DateTime EstimatedDeliveryDate { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime LastUpdatedOn { get; set; }
    public string LastOpsStation { get; set; }
    public string LastAction { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public string ActualStep { get; set; }
    public DateTime? DeliveryDateTime { get; set; }
    public string ServiceTakerCode { get; set; }
    public string ServiceTakerName { get; set; }
    public AbbreviatedStatesEnum State { get; set; }

    [BsonIgnore]
    public PerformanceEnum Performance { get; set; }
}
