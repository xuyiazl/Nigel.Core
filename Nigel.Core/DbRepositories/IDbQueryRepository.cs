using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Nigel.Core.Collection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nigel.Core.DbRepositories
{

    public interface IDbQueryRepository<TEntity> where TEntity : class
    {


        DbContext Context { get; }
        DbSet<TEntity> Table { get; }
        DatabaseFacade Database { get; }
        bool IsNoTracking { get; set; }
        IQueryable<TEntity> AsNoTracking();
        EntityEntry Entry([NotNull] object entity);
        EntityEntry<TEntity> Entry([NotNull] TEntity entity);
        bool Any(Expression<Func<TEntity, bool>> filter = null);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter = null);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter = null, CancellationToken cancellationToken = default);
        int Count(Expression<Func<TEntity, bool>> filter = null);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null, CancellationToken cancellationToken = default);
        TEntity GetEntity(Expression<Func<TEntity, bool>> selector = null);
        TEntity GetEntity<TOrder>(Expression<Func<TEntity, bool>> selector = null, Expression<Func<TEntity, TOrder>> orderBy = null, bool orderDesc = false);
        TResult GetEntity<TResult, TOrder>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector = null, Expression<Func<TEntity, TOrder>> orderBy = null, bool orderDesc = false);
        TResult GetEntity<TResult>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector = null);
        Task<TEntity> GetEntityAsync(Expression<Func<TEntity, bool>> selector = null);
        Task<TEntity> GetEntityAsync(Expression<Func<TEntity, bool>> selector = null, CancellationToken cancellationToken = default);
        Task<TEntity> GetEntityAsync<TOrder>(Expression<Func<TEntity, bool>> selector = null, Expression<Func<TEntity, TOrder>> orderBy = null, bool orderDesc = false);
        Task<TEntity> GetEntityAsync<TOrder>(Expression<Func<TEntity, bool>> selector = null, Expression<Func<TEntity, TOrder>> orderBy = null, bool orderDesc = false, CancellationToken cancellationToken = default);
        Task<TResult> GetEntityAsync<TResult, TOrder>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector = null, Expression<Func<TEntity, TOrder>> orderBy = null, bool orderDesc = false);
        Task<TResult> GetEntityAsync<TResult, TOrder>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector = null, Expression<Func<TEntity, TOrder>> orderBy = null, bool orderDesc = false, CancellationToken cancellationToken = default);
        Task<TResult> GetEntityAsync<TResult>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector = null);
        Task<TResult> GetEntityAsync<TResult>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector = null, CancellationToken cancellationToken = default);
        IList<TEntity> GetList(Expression<Func<TEntity, bool>> selector = null);
        IList<TEntity> GetList<TOrder>(Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TOrder>> orderBy = null, bool orderDesc = false);
        IList<TResult> GetList<TResult>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector = null);
        Task<IList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> selector = null);
        Task<IList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> selector = null, CancellationToken cancellationToken = default);
        Task<IList<TEntity>> GetListAsync<TOrder>(Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TOrder>> orderBy = null, bool orderDesc = false);
        Task<IList<TEntity>> GetListAsync<TOrder>(Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TOrder>> orderBy = null, bool orderDesc = false, CancellationToken cancellationToken = default);
        Task<IList<TResult>> GetListAsync<TResult>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector = null);
        Task<IList<TResult>> GetListAsync<TResult>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector = null, CancellationToken cancellationToken = default);
        PagedList<TResult> GetPagedList<TOrder, TResult>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TOrder>> orderBy = null, bool orderDesc = false, int pageNumber = 1, int pageSize = 10);
        PagedList<TEntity> GetPagedList<TOrder>(Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TOrder>> orderBy = null, bool orderDesc = false, int pageNumber = 1, int pageSize = 10);
        Task<PagedList<TResult>> GetPagedListAsync<TOrder, TResult>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TOrder>> orderBy = null, bool orderDesc = false, int pageNumber = 1, int pageSize = 10);
        Task<PagedList<TResult>> GetPagedListAsync<TOrder, TResult>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TOrder>> orderBy = null, bool orderDesc = false, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);
        Task<PagedList<TEntity>> GetPagedListAsync<TOrder>(Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TOrder>> orderBy = null, bool orderDesc = false, int pageNumber = 1, int pageSize = 10);
        Task<PagedList<TEntity>> GetPagedListAsync<TOrder>(Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TOrder>> orderBy = null, bool orderDesc = false, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);
        IList<TEntity> SqlQuery(string sql, params object[] parameters);
        IList<TResult> SqlQuery<TResult>(Expression<Func<TEntity, TResult>> converter, string sql, params object[] parameters);
        Task<IList<TEntity>> SqlQueryAsync(string sql, CancellationToken cancellationToken = default, params object[] parameters);
        Task<IList<TEntity>> SqlQueryAsync(string sql, params object[] parameters);
        Task<IList<TResult>> SqlQueryAsync<TResult>(Expression<Func<TEntity, TResult>> converter, string sql, CancellationToken cancellationToken = default, params object[] parameters);
        Task<IList<TResult>> SqlQueryAsync<TResult>(Expression<Func<TEntity, TResult>> converter, string sql, params object[] parameters);
    }
}
