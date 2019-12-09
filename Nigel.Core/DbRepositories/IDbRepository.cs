using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Nigel.Core.Collection;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nigel.Core.DbRepositories
{

    public interface IDbRepository<TEntity> where TEntity : class
    {
        DbContext Context { get; set; }

        void Add(TEntity entity);
        void AddAsync(TEntity entity);
        void AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        void AddRange(IList<TEntity> Entities);
        void AddRangeAsync(IList<TEntity> Entities);
        void AddRangeAsync(IList<TEntity> Entities, CancellationToken cancellationToken = default);
        IDbContextTransaction BeginTransaction();
        void CommitTransaction();
        void Delete(Expression<Func<TEntity, bool>> selector);
        void Delete(TEntity Entity);
        void DeleteRange(Expression<Func<TEntity, bool>> selector);
        void DeleteRange(IList<TEntity> entities);
        TEntity GetEntity(Expression<Func<TEntity, bool>> selector = null);
        TResult GetEntity<TResult>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector = null);
        Task<TEntity> GetEntityAsync(Expression<Func<TEntity, bool>> selector = null);
        Task<TEntity> GetEntityAsync(Expression<Func<TEntity, bool>> selector = null, CancellationToken cancellationToken = default);
        Task<TResult> GetEntityAsync<TResult>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector = null);
        Task<TResult> GetEntityAsync<TResult>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector = null, CancellationToken cancellationToken = default);
        IList<TEntity> GetList(Expression<Func<TEntity, bool>> selector = null);
        IList<TEntity> GetList<TKey>(Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TKey>> orderBy = null);
        IList<TResult> GetList<TResult>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector = null);
        Task<IList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> selector = null);
        Task<IList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> selector = null, CancellationToken cancellationToken = default);
        Task<IList<TEntity>> GetListAsync<TKey>(Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TKey>> orderBy = null);
        Task<IList<TEntity>> GetListAsync<TKey>(Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TKey>> orderBy = null, CancellationToken cancellationToken = default);
        Task<IList<TResult>> GetListAsync<TResult>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector = null);
        Task<IList<TResult>> GetListAsync<TResult>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector = null, CancellationToken cancellationToken = default);
        PagedList<TResult> GetPagedList<TKey, TResult>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TKey>> orderBy = null, int pageNumber = 1, int pageSize = 10);
        PagedList<TEntity> GetPagedList<TKey>(Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TKey>> orderBy = null, int pageNumber = 1, int pageSize = 10);
        Task<PagedList<TResult>> GetPagedListAsync<TKey, TResult>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TKey>> orderBy = null, int pageNumber = 1, int pageSize = 10);
        Task<PagedList<TResult>> GetPagedListAsync<TKey, TResult>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TKey>> orderBy = null, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);
        Task<PagedList<TEntity>> GetPagedListAsync<TKey>(Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TKey>> orderBy = null, int pageNumber = 1, int pageSize = 10);
        Task<PagedList<TEntity>> GetPagedListAsync<TKey>(Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TKey>> orderBy = null, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);
        TResult GetResult<TResult>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector);
        Task<TResult> GetResultAsync<TResult>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector);
        Task<TResult> GetResultAsync<TResult>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector, CancellationToken cancellationToken = default);
        void RollbackTransaction();
        bool Save();
        Task<bool> SaveAsync();
        Task<bool> SaveAsync(CancellationToken cancellationToken = default);
        IList<TEntity> SqlQuery(string sql, params object[] parameters);
        IList<TResult> SqlQuery<TResult>(Expression<Func<TEntity, TResult>> converter, string sql, params object[] parameters);
        Task<IList<TEntity>> SqlQueryAsync(string sql, CancellationToken cancellationToken = default, params object[] parameters);
        Task<IList<TEntity>> SqlQueryAsync(string sql, params object[] parameters);
        Task<IList<TResult>> SqlQueryAsync<TResult>(Expression<Func<TEntity, TResult>> converter, string sql, CancellationToken cancellationToken = default, params object[] parameters);
        Task<IList<TResult>> SqlQueryAsync<TResult>(Expression<Func<TEntity, TResult>> converter, string sql, params object[] parameters);
        void Update(TEntity entity);
        void UpdateRange(IList<TEntity> entities);
    }
}
