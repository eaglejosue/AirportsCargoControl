namespace Tests.Utils;

public static class Extensions
{
    public static string CreateQueryString(this object obj)
    {
        var queryString = string.Empty;

        var properties = from p in obj.GetType().GetProperties()
                         where p.GetValue(obj, null) != null
                         select p;

        foreach (var p in properties)
        {
            if (p is null) continue;

            if (p.PropertyType.IsArray)
            {
                var elementType = p.PropertyType.GetElementType();
                if (elementType == null) continue;

                var arrayValues = Array.Empty<string>();

                // Array de string
                if (elementType.Equals(typeof(string)))
                {
                    var values = p.GetValue(obj);
                    if (values != null)
                        arrayValues = (string[])values;
                }

                // Array de int
                if (elementType.Equals(typeof(int)))
                {
                    var values = p.GetValue(obj);
                    if (values != null)
                        arrayValues = (from v in (int[])values select v.ToString()).ToArray();
                }

                // Array de long
                if (elementType.Equals(typeof(long)))
                {
                    var values = p.GetValue(obj);
                    if (values != null)
                        arrayValues = (from v in (long[])values select v.ToString()).ToArray();
                }

                foreach (var value in arrayValues)
                    queryString += p.Name + "=" + HttpUtility.UrlEncode(value) + "&";
            }
            else
            {
                var value = (p?.GetValue(obj, null) ?? string.Empty).ToString();
                if (!string.IsNullOrWhiteSpace(value)) queryString += p?.Name + "=" + HttpUtility.UrlEncode(value) + "&";
            }
        }

        // Remove último "&"
        if (queryString.Length > 1)
            queryString = "?" + queryString.Remove(queryString.Length - 1);

        return queryString;
    }
}
