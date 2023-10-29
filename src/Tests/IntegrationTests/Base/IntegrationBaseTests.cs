namespace Tests.IntegrationTests.Base;

public class IntegrationBaseTests
{
    public static Task DefaultForPaginationResultAssertsAsync<T>(HttpResponseMessage response) where T : class
        => DefaultResultAssertsAsync<T>(response, true);

    public static async Task DefaultResultAssertsAsync<T>(HttpResponseMessage response, bool usePaginationResult = false) where T : class
    {
        response.StatusCode.Should().NotBe(HttpStatusCode.InternalServerError);

        var jsonString = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrEmpty(jsonString))
        {
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            return;
        }

        if (usePaginationResult)
        {
            var pageResult = JObject.Parse(jsonString).ToObject<PaginationResult<T>>();
            if (pageResult.Results.Count == 0)
            {
                response.StatusCode.Should().Be(HttpStatusCode.NoContent);
                return;
            }

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            pageResult.Results.Count.Should().BeGreaterThan(0);

            return;
        }

        var listResult = JObject.Parse(jsonString).ToObject<List<T>>();
        if (listResult.Count == 0)
        {
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            return;
        }

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        listResult.Count.Should().BeGreaterThan(0);
    }
}
