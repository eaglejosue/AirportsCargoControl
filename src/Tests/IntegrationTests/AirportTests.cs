namespace Tests.IntegrationTests;

public class LocationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;
    const string urlPrefix = "api/airport";

    public LocationTests(WebApplicationFactory<Program> factory)
    {
        _httpClient = factory.CreateClient();
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task GetAuditLogByLocationPaged(bool useSql)
    {
        //Arrange
        var query = new
        {
            RangeDateInitial = "2022-12-28",//DateTime.Today.ToString("yyyy-MM-dd"),
            RangeDateFinal = "2022-12-28",//DateTime.Today.ToString("yyyy-MM-dd"),
            NameLocation = "VCP",
            Page = 1,
            PageSize = 10,
            UseSql = useSql
        };

        // Act
        var response = await _httpClient.GetAsync($"{urlPrefix}{query.CreateQueryString()}");

        // Assert
        await IntegrationBaseTests.DefaultForPaginationResultAssertsAsync<Cargo>(response);
    }
}