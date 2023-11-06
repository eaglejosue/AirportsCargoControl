namespace Core.Data.Entities;

[MongoCollection("Awb")]
public sealed class Cargo : Document<Guid>
{
    public string Code { get; set; }
    public DateTime EstimatedDeliveryDate { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
    public string AirportIsOn { get; set; }
    public AbbreviatedStatesEnum StateIsOn { get; set; }
    public string LastAction { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public string Step { get; set; }
    public DateTime? OccurrenceDate { get; set; }
    public DateTime? DeliveryDateTime { get; set; }
    public string ClientCode { get; set; }
    public string ClientName { get; set; }

    [BsonIgnore]
    public PerformanceEnum Performance { get; set; }
}
