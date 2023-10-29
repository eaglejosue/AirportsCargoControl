namespace Api.Endpoints;

public static class VersionEndpoints
{
    public static void ConfigureVersionEndpoints(this WebApplication app)
    {
        app.MapGet("api/version",
        ([FromServices] IConfiguration configuration) =>
        {
            var version = Assembly.GetEntryAssembly().GetName().Version.ToString();
            return Results.Ok(version);
        });
    }
}
