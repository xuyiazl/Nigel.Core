using Nigel.Paging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nigel.Data.DbService
{

    /// <summary>
    /// 通用仓储库的方法定义
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseRepository<T> where T : class, new()
    {
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        int Insert(T entity, bool isSaveChange = true);
        Task<int> InsertAsync(T entity, bool isSaveChange = true, CancellationToken cancellationToken = default);
        int BatchInsert(T[] entities, bool isSaveChange = true);
        Task<int> BatchInsertAsync(T[] entities, bool isSaveChange = true, CancellationToken cancellationToken = default);
        int Update(T entity, bool isSaveChange = true);
        Task<int> UpdateAsync(T entity, bool isSaveChange = true, CancellationToken cancellationToken = default);
        int Update(T entity, List<string> updatePropertyList, bool modified = true, bool isSaveChange = true);
        Task<int> UpdateAsync(T entity, List<string> updatePropertyList, bool modified = true, bool isSaveChange = true, CancellationToken cancellationToken = default);
        int BatchUpdate(T[] entities, bool isSaveChange = true);
        Task<int> BatchUpdateAsync(T[] entities, bool isSaveChange = true, CancellationToken cancellationToken = default);
        int Delete(T entity, bool isSaveChange = true);
        Task<int> DeleteAsync(T entity, bool isSaveChange = true, CancellationToken cancellationToken = default);
        int BatchDelete(T[] entities, bool isSaveChange = true);
        Task<int> BatchDeleteAsync(T[] entities, bool isSaveChange = true, CancellationToken cancellationToken = default);
        T GetById(object id);
        Task<T> GetByIdAsync(object id);
        List<T> GetList();
        Task<List<T>> GetListAsync();
        List<T> GetList(string orderby);
        Task<List<T>> GetListAsync(string orderby);
        List<T> GetList(Expression<Func<T, bool>> selector);
        Task<List<T>> GetListAsync(Expression<Func<T, bool>> selector);
        List<T> GetList(Expression<Func<T, bool>> selector, string orderby);
        Task<List<T>> GetListAsync(Expression<Func<T, bool>> selector, string orderby);
        List<T> GetList(Expression<Func<T, bool>> selector, int skip = 0, int limit = 20);
        Task<List<T>> GetListAsync(Expression<Func<T, bool>> selector, int skip = 0, int limit = 20);
        List<T> GetList(Expression<Func<T, bool>> selector, string orderby, int skip = 0, int limit = 20);
        Task<List<T>> GetListAsync(Expression<Func<T, bool>> selector, string orderby, int skip = 0, int limit = 20);
        PagedSkipModel<T> GetPagedSkipList(Expression<Func<T, bool>> selector, string orderby, int skip = 0, int limit = 20);
        Task<PagedSkipModel<T>> GetPagedSkipListAsync(Expression<Func<T, bool>> selector, string orderby, int skip = 0, int limit = 20);
        PagedModel<T> GetPagedList(Expression<Func<T, bool>> selector, string orderby, int pageNumber = 1, int pageSize = 20);
        Task<PagedModel<T>> GetPagedListAsync(Expression<Func<T, bool>> selector, string orderby, int pageNumber = 1, int pageSize = 20);
        bool Any(Expression<Func<T, bool>> selector);
        Task<bool> AnyAsync(Expression<Func<T, bool>> selector);
        int GetCount(Expression<Func<T, bool>> selector);
        Task<int> GetCountAsync(Expression<Func<T, bool>> selector);

    }
}
