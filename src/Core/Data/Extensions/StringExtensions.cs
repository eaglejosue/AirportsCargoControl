namespace Core.Data.Extensions;

public static class StringExtensions
{
    public static string ReplaceDotForCommaAndCommaForDot(this string Source)
    {
        var temp = Source.Replace(".", "<DOT>");
        var temp2 = temp.Replace(",", ".");
        return temp2.Replace("<DOT>", ",");
    }

    public static string RemoveCharacters(this string value, char[] toBeRemoved) =>
       string.IsNullOrWhiteSpace(value) ? value : string.Join("", value.Split(toBeRemoved));

    public static string RemoveDiacritics(this string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;

        text = text.Normalize(NormalizationForm.FormD);
        var chars = text.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
        return new string(chars).Normalize(NormalizationForm.FormC);
    }

    public static List<TEnum> GetListOfEnum<TEnum>(this string source) where TEnum : struct
    {
        var listOfEnum = new List<TEnum>();
        if (string.IsNullOrWhiteSpace(source))
            return listOfEnum;

        var list = GetListOfString(source);
        foreach (var item in list)
        {
            if (Enum.TryParse(item, true, out TEnum result))
                listOfEnum.Add(result);
        }

        return listOfEnum;
    }

    public static List<string> GetListOfString(this string source)
    {
        if (string.IsNullOrWhiteSpace(source))
            return new List<string>();

        return source.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(item => item?.Trim()).ToList();
    }

    public static string ConcatenateWithComma(this string source, string value)
    {
        if (!string.IsNullOrWhiteSpace(source) && !string.IsNullOrWhiteSpace(value))
            source += ",";

        return source += value;
    }

    public static string KeyedObject<T>(this T obj, int maxLength)
    {
        PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        string result = "";

        foreach (PropertyInfo prop in properties)
        {
            object value = prop.GetValue(obj);

            if (prop.GetCustomAttribute<JsonIgnoreAttribute>() != null)
                continue;

            if (value != null && !value.Equals(0))
            {
                string stringValue = value.ToString();

                if (prop.PropertyType == typeof(DateTime))
                {
                    DateTime dateTimeValue = (DateTime)value;
                    stringValue = dateTimeValue.ToString("yyyy-MM-dd");
                }

                result += $"{stringValue},";
            }
        }

        if (!string.IsNullOrEmpty(result))
        {
            result = result.Remove(result.Length - 1);
        }

        if (result.Length > maxLength)
        {
            result = string.Join(",", result.Split(',').Take(properties.Length));
        }

        return result;
    }
}
