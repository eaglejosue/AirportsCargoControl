namespace Core.Data.Pagination;

public sealed class PaginationResult<T> : PaginationBase where T : class
{
    public List<T> Results { get; set; }

    public PaginationResult()
    {
        Results = new List<T>();
    }

    public PaginationResult(int currentPage, int itemsPerPage, int total, List<T> results = null) : base
        (currentPage, itemsPerPage, total)
    {
        Results = results ?? new List<T>();

        if (total == 0 && results.Count > 0)
        {
            Total = results.Count;
            TotalPages = (int)Math.Ceiling(itemsPerPage > 0 ? (double)total / itemsPerPage : 1);
        }
    }
}

public class PaginationBase
{
    public int CurrentPage { get; set; }
    public int ItemsPerPage { get; set; }
    public int Total { get; set; }
    public int TotalPages { get; set; }
    public int FirstLineOnPage => (CurrentPage - 1) * ItemsPerPage + 1;
    public int LastLineOfPage => Math.Min(CurrentPage * ItemsPerPage, Total);

    public PaginationBase() { }

    public PaginationBase(int currentPage, int itemsPerPage, int total)
    {
        CurrentPage = currentPage;
        ItemsPerPage = itemsPerPage;
        Total = total;

        if (total > 0)
            TotalPages = (int)Math.Ceiling(itemsPerPage > 0 ? (double)total / itemsPerPage : 1);
    }
}
