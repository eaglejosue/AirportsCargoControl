namespace Core.Data.Repositories.Mongo;

public interface ICargoMongoRepository : IMongoGenericRepository<Cargo, Guid> { }

public sealed class CargoMongoRepository : MongoGenericRepository<Cargo, Guid>, ICargoMongoRepository
{
    public CargoMongoRepository(IMongoDbSettings settings) : base(settings) { }
}
