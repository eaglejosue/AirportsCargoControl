namespace Core.Data.Dtos
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class ResponseObject<TDataObject>
    {
        [JsonProperty("data")]
        public TDataObject Data { get; set; }

        [JsonProperty("notifications")]
        public IDictionary<string, object> Notifications { get; set; } = new ConcurrentDictionary<string, object>();
    }
}
