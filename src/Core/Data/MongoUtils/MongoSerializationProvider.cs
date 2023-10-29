namespace Core.Data.MongoUtils;

public class MongoSerializationProvider : IBsonSerializationProvider
{
    public IBsonSerializer? GetSerializer(Type type)
    {
        if (type == typeof(DateTime))
            return new MongoDateTimeSerializer();

        return null;
    }
}

public class MongoDateTimeSerializer : DateTimeSerializer
{
    public override DateTime Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        try
        {
            var obj = base.Deserialize(context, args);
            return new DateTime(obj.Ticks, DateTimeKind.Unspecified);
        }
        catch (Exception)
        {
            return DateTime.MinValue;
        }
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateTime value)
    {
        var utcValue = new DateTime(value.Ticks, DateTimeKind.Utc);
        base.Serialize(context, args, utcValue);
    }
}
