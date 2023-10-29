namespace Core.Data.Repositories.Mongo.Base;

public interface IMongoGenericRepository<TDocument, TIdentifier> where TDocument : IDocument<TIdentifier>
{
    IQueryable<TDocument> AsQueryable();

    Task<List<TDocument>> FilterByAsync(Expression<Func<TDocument, bool>> filterExpression);

    Task<List<TProjected>> FilterByAsync<TProjected>(
        Expression<Func<TDocument, bool>> filterExpression,
        Expression<Func<TDocument, TProjected>> projectionExpression);

    TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression);

    Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression);

    TDocument FindById(TIdentifier identifier);

    Task<TDocument> FindByIdAsync(TIdentifier identifier);

    void InsertOne(TDocument document);

    Task InsertOneAsync(TDocument document);

    void InsertMany(List<TDocument> documents);

    Task InsertManyAsync(List<TDocument> documents);

    void ReplaceOne(TDocument document);

    Task ReplaceOneAsync(TDocument document);

    void DeleteOne(Expression<Func<TDocument, bool>> filterExpression);

    Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression);

    void DeleteById(TIdentifier identifier);

    Task DeleteByIdAsync(TIdentifier identifier);

    void DeleteMany(Expression<Func<TDocument, bool>> filterExpression);

    Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression);
}
