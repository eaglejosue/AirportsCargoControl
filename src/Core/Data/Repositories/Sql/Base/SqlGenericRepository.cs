namespace Core.Data.Repositories.Sql.Base;

public interface ISqlGenericRepository<T> where T : class
{
    Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    Task<List<T>> SelectAsync(Expression<Func<T, bool>> predicate);
    Task InsertAsync(T entity);
    Task<bool> UpdateAsync(T entity);
    Task<int> DeleteMultipleAsync(Expression<Func<T, bool>> predicate);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    Task InsertListAsync(List<T> list);
    Task<bool> UpdateListAsync(List<T> list);
    Task BulkInsertAsync(List<T> items, int pageSize = 1000);
    Task BulkUpdateAsync(List<T> items, int pageSize = 1000);
}

public abstract class SqlGenericRepository<T> : ISqlGenericRepository<T> where T : class
{
    protected SqlSession _sqlSession;

    public SqlGenericRepository(SqlSession sqlSession)
    {
        _sqlSession = sqlSession;
    }

    public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate) => _sqlSession.Connection.FirstOrDefaultAsync(predicate);

    public async Task<List<T>> SelectAsync(Expression<Func<T, bool>> predicate)
    {
        var result = (await _sqlSession.Connection.SelectAsync(predicate)).ToList();
        result.Capacity = result.Count;
        return result;
    }

    public Task InsertAsync(T entity) => _sqlSession.Connection.InsertAsync(entity);

    public Task<bool> UpdateAsync(T entity) => _sqlSession.Connection.UpdateAsync(entity);

    public Task<int> DeleteMultipleAsync(Expression<Func<T, bool>> predicate) => _sqlSession.Connection.DeleteMultipleAsync(predicate);

    public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate) => _sqlSession.Connection.AnyAsync(predicate);

    public Task InsertListAsync(List<T> list) => _sqlSession.Connection.InsertAllAsync(list);

    public Task<bool> UpdateListAsync(List<T> list) => _sqlSession.Connection.UpdateAsync(list);

    public Task BulkInsertAsync(List<T> list, int pageSize = 1000)
    {
        var page = 0;
        var listInsert = list.Skip(page * pageSize).Take(pageSize).ToList();

        while (listInsert.Count > 0)
        {
            try
            {
                _sqlSession.Connection.BulkInsert(listInsert);
            }
            catch (Exception ex)
            {
                var error = ex;
            }

            page++;
            listInsert = list.Skip(page * pageSize).Take(pageSize).ToList();
        };

        return Task.CompletedTask;
    }

    public Task BulkUpdateAsync(List<T> items, int pageSize = 1000)
    {
        var page = 0;
        var listUpdate = items.Skip(page * pageSize).Take(pageSize).ToList();

        while (listUpdate.Count > 0)
        {
            try
            {
                _sqlSession.Connection.BulkUpdate(listUpdate);
            }
            catch (Exception ex)
            {
                var error = ex;
            }

            page++;
            listUpdate = items.Skip(page * pageSize).Take(pageSize).ToList();
        };

        return Task.CompletedTask;
    }
}
