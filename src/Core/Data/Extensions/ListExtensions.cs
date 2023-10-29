namespace Core.Data.Extensions;

public static class ListExtensions
{
    public static PaginationResult<T> GetPaged<T>(this IEnumerable<T> query, int currentPage, int itemsPerPage) where T : class
    {
        var total = query.Count();
        var skip = (currentPage - 1) * itemsPerPage;
        var results = itemsPerPage > 0
            ? query.Skip(skip).Take(itemsPerPage).ToList()
            : query.Skip(skip).ToList();

        return new PaginationResult<T>(currentPage, itemsPerPage, total, results);
    }

    public static PaginationResult<T> GetPaged<T>(this IEnumerable<T> query, int currentPage, int itemsPerPage, int countDB) where T : class =>
        new(currentPage, itemsPerPage, countDB, query.ToList());

    public static List<T> OrderByPropertieName<T>(this List<T> items, string orderByPropertieName = null, bool orderByDescending = false)
    {
        if (string.IsNullOrWhiteSpace(orderByPropertieName))
            return items;

        var propertyInfo = typeof(T).GetProperties().FirstOrDefault(f => f.Name.ToLowerInvariant().Contains(orderByPropertieName.ToLowerInvariant()));
        if (propertyInfo == null)
            return items;

        return orderByDescending
            ? items.OrderByDescending(o => propertyInfo.GetValue(o, null)).ToList()
            : items.OrderBy(o => propertyInfo.GetValue(o, null)).ToList();
    }

    public static IEnumerable<T> OrderByDictionary<T>(this IEnumerable<T> source, Dictionary<string, int> orderDict, Func<T, string> keySelector)
    {
        var result = from item in source
                     join order in orderDict on keySelector(item) equals order.Key into orderGroup
                     from subOrder in orderGroup.DefaultIfEmpty(new KeyValuePair<string, int>("", int.MaxValue))
                     orderby subOrder.Value
                     select item;

        return result;
    }
}
