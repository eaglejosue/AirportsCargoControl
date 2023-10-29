namespace Core.Data.Repositories.Sql;

public interface ICargoSqlRepository : ISqlGenericRepository<Cargo> { }

public sealed class CargoSqlRepository : SqlGenericRepository<Cargo>, ICargoSqlRepository
{
    public CargoSqlRepository(SqlSession sqlSession) : base(sqlSession) { }
}
