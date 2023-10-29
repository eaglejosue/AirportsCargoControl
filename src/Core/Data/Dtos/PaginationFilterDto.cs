namespace Core.Data.Dtos;

public interface IPaginationFilterDto
{
    int PageSize { get; set; }
    int Page { get; set; }
    string OrderBy { get; set; }
    bool OrderByDescending { get; set; }
}

public sealed class PaginationFilterDto : IPaginationFilterDto
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string OrderBy { get; set; }
    public bool OrderByDescending { get; set; }

    public PaginationFilterDto() { }
    public PaginationFilterDto(int page, int pageSize, string orderBy, bool orderByDescending)
    {
        Page = page;
        PageSize = pageSize;
        OrderBy = orderBy ?? string.Empty;
        OrderByDescending = orderByDescending;
    }

    public static ValueTask<PaginationFilterDto> BindAsync(HttpContext context)
        => ValueTask.FromResult<PaginationFilterDto>(new(
            int.TryParse(context.Request.Query["Page"], out var page) ? page : 1,
            int.TryParse(context.Request.Query["PageSize"], out var pageSize) ? pageSize : 10,
            context.Request.Query["OrderBy"].ToString(),
            bool.TryParse(context.Request.Query["orderByDescending"], out var orderByDescending) && orderByDescending));
}
