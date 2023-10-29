namespace Core.Data.Extensions;

public static class JsonExtensions
{
    private static readonly UTF8Encoding Utf8NoBom = new(encoderShouldEmitUTF8Identifier: false);

    private static readonly JsonSerializerSettings JsonSettings = new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        DateFormatHandling = DateFormatHandling.IsoDateFormat,
        NullValueHandling = NullValueHandling.Ignore,
        DefaultValueHandling = DefaultValueHandling.Ignore,
        MissingMemberHandling = MissingMemberHandling.Ignore,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        TypeNameHandling = TypeNameHandling.None,
        Converters = new JsonConverter[1] { new StringEnumConverter() }
    };

    public static byte[]? ToJsonBytes(this object source)
    {
        if (source == null)
            return null;

        var s = JsonConvert.SerializeObject(source, Formatting.Indented, JsonSettings);
        return Utf8NoBom.GetBytes(s);
    }

    public static string? ToJson(this object source)
    {
        if (source == null)
            return null;

        return JsonConvert.SerializeObject(source, Formatting.Indented, JsonSettings);
    }

    public static string? ToJson(this object source, JsonSerializerSettings jsonSettings)
    {
        if (source == null)
            return null;

        if (jsonSettings == null)
            throw new ArgumentNullException(nameof(jsonSettings));

        jsonSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        return JsonConvert.SerializeObject(source, Formatting.Indented, jsonSettings);
    }

    public static string? ToCanonicalJson(this object source)
    {
        if (source == null)
            return null;

        return JsonConvert.SerializeObject(source, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
    }

    public static T? ParseJson<T>(this string json)
    {
        if (string.IsNullOrEmpty(json))
            return default;

        return JsonConvert.DeserializeObject<T>(json, JsonSettings);
    }

    public static T? ParseJson<T>(this string json, JsonSerializerSettings settings)
    {
        if (string.IsNullOrEmpty(json))
            return default;

        if (settings == null)
            throw new ArgumentNullException(nameof(settings));

        return JsonConvert.DeserializeObject<T>(json, settings);
    }

    public static T? ParseJson<T>(this byte[] json)
    {
        if (json == null || json.Length == 0)
        {
            return default;
        }

        return JsonConvert.DeserializeObject<T>(Utf8NoBom.GetString(json), JsonSettings);
    }

    public static T? ParseJson<T>(this byte[] json, JsonSerializerSettings settings)
    {
        if (json == null || json.Length == 0)
            return default;

        if (settings == null)
            throw new ArgumentNullException(nameof(settings));

        return JsonConvert.DeserializeObject<T>(Utf8NoBom.GetString(json), settings);
    }

    public static object JsonDeserializeObject(JObject value, Type type, JsonSerializerSettings settings) =>
        JsonSerializer.Create(settings).Deserialize(new JTokenReader(value), type);
}
