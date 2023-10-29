namespace Core.Data.Repositories.Mongo.Base;

public abstract class MongoGenericRepository<TDocument, TIdentifier> : IMongoGenericRepository<TDocument, TIdentifier> where TDocument : IDocument<TIdentifier>, new()
{
    protected readonly IMongoCollection<TDocument> _collection;

    public MongoGenericRepository(IMongoDbSettings settings)
    {
        var database = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);
        _collection = database.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));
    }

    private protected static string GetCollectionName(Type documentType) =>
        ((MongoCollectionAttribute)documentType.GetCustomAttributes(typeof(MongoCollectionAttribute), true).FirstOrDefault())?.CollectionName;

    public virtual IQueryable<TDocument> AsQueryable() => _collection.AsQueryable();

    public virtual Task<List<TDocument>> FilterByAsync(Expression<Func<TDocument, bool>> filterExpression) => _collection.Find(filterExpression).ToListAsync();

    public virtual Task<List<TProjected>> FilterByAsync<TProjected>(
        Expression<Func<TDocument, bool>> filterExpression,
        Expression<Func<TDocument, TProjected>> projectionExpression) =>
        _collection.Find(filterExpression).Project(projectionExpression).ToListAsync();

    public virtual TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression) => _collection.Find(filterExpression).FirstOrDefault();

    public virtual Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression) => _collection.Find(filterExpression).FirstOrDefaultAsync();

    public virtual TDocument FindById(TIdentifier identifier) => _collection.Find(Builders<TDocument>.Filter.Eq(doc => doc.Id, identifier)).SingleOrDefault();

    public virtual Task<TDocument> FindByIdAsync(TIdentifier identifier) => _collection.Find(Builders<TDocument>.Filter.Eq(doc => doc.Id, identifier)).SingleOrDefaultAsync();

    public virtual void InsertOne(TDocument document) => _collection.InsertOne(document);

    public virtual Task InsertOneAsync(TDocument document) => _collection.InsertOneAsync(document);

    public void InsertMany(List<TDocument> documents) => _collection.InsertMany(documents);

    public virtual Task InsertManyAsync(List<TDocument> documents) => _collection.InsertManyAsync(documents);

    public void ReplaceOne(TDocument document) => _collection.FindOneAndReplace(Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id), document);

    public virtual Task ReplaceOneAsync(TDocument document) => _collection.FindOneAndReplaceAsync(Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id), document);

    public void DeleteOne(Expression<Func<TDocument, bool>> filterExpression) => _collection.FindOneAndDelete(filterExpression);

    public Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression) => _collection.FindOneAndDeleteAsync(filterExpression);

    public void DeleteById(TIdentifier identifier) => _collection.FindOneAndDelete(Builders<TDocument>.Filter.Eq(doc => doc.Id, identifier));

    public Task DeleteByIdAsync(TIdentifier identifier) => _collection.FindOneAndDeleteAsync(Builders<TDocument>.Filter.Eq(doc => doc.Id, identifier));

    public void DeleteMany(Expression<Func<TDocument, bool>> filterExpression) => _collection.DeleteMany(filterExpression);

    public Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression) => _collection.DeleteManyAsync(filterExpression);
}
