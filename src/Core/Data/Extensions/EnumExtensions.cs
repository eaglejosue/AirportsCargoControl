namespace Core.Data.Extensions;

public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        var descriptionAttribute = value.GetType().GetField(value.ToString())!.GetCustomAttributes(typeof(DescriptionAttribute), inherit: false).SingleOrDefault() as DescriptionAttribute;
        if (descriptionAttribute != null)
            return descriptionAttribute.Description;

        return value.ToString();
    }
}
