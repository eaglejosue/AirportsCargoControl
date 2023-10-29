namespace Core.Data.Models;

[MongoCollection("AirportLocation")]
public sealed class Airport : Document<Guid>
{
    public string AirportCode { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public BrazilianRegionsEnum Region { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public BrazilianStatesEnum State { get; set; }
    public string City { get; set; }
    public double Lat { get; set; }
    public double Long { get; set; }
    public string Address { get; set; }
}
