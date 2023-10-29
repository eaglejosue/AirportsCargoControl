namespace Core.Data.Repositories.Sql.Base;

public interface IUnitOfWork : IDisposable
{
    void BeginTransaction();
    void Commit();
    void Rollback();
}

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly SqlSession _sqlSession;

    public UnitOfWork(SqlSession sqlSession)
    {
        _sqlSession = sqlSession;
    }

    public void BeginTransaction() => _sqlSession.Transaction = _sqlSession.Connection.BeginTransaction();

    public void Commit()
    {
        _sqlSession.Transaction.Commit();
        Dispose();
    }

    public void Rollback()
    {
        _sqlSession.Transaction.Rollback();
        Dispose();
    }

    public void Dispose() => _sqlSession.Transaction?.Dispose();
}
