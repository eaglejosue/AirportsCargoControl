namespace Core.Data.Entities.Base;

public interface IDocument<TIdentification>
{
    [BsonId]
    TIdentification Id { get; set; }
}

[BsonIgnoreExtraElements]
public abstract class Document<TIdentification> : IDocument<TIdentification>
{
    public TIdentification Id { get; set; }
}
