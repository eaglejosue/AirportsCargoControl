namespace Core.Data.Repositories.Sql.Base;

public sealed class SqlSession : IDisposable
{
    public IDbConnection Connection { get; }
    public IDbTransaction Transaction { get; set; }

    public SqlSession(string connectionString)
    {
        Connection = new SqlConnection(connectionString);
        Connection.Open();
    }

    public void Dispose() => Connection?.Dispose();
}
