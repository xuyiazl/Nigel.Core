using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Nigel.Core.Collection;

namespace Nigel.Core.DbRepositories
{
    public class DbRepository<TEntity> : IDbQueryRepository<TEntity>, IDbChangeRepository<TEntity>, IDbSaveRepository<TEntity>
        where TEntity : class
    {
        public DbContext Context { set; get; }

        public DbRepository(DbContext dbContext)
        {
            Context = dbContext;
        }

        #region [ IDbSaveRepository ]

        public IDbContextTransaction BeginTransaction()
        {
            return Context.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            Context.Database.CommitTransaction();
        }

        public void RollbackTransaction()
        {
            Context.Database.RollbackTransaction();
        }

        public bool Save()
        {
            return Context.SaveChanges() > 0;
        }

        public async Task<bool> SaveAsync()
        {
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<bool> SaveAsync(CancellationToken cancellationToken = default)
        {
            return await Context.SaveChangesAsync(cancellationToken) > 0;
        }

        #endregion

        #region [ IDbChangeRepository ] 

        public void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }

        public async void AddAsync(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
        }

        public async void AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await Context.Set<TEntity>().AddAsync(entity, cancellationToken);
        }

        public async void AddRange(IList<TEntity> Entities)
        {
            await Context.Set<TEntity>().AddRangeAsync(Entities);
        }

        public async void AddRangeAsync(IList<TEntity> Entities)
        {
            await Context.Set<TEntity>().AddRangeAsync(Entities);
        }

        public async void AddRangeAsync(IList<TEntity> Entities, CancellationToken cancellationToken = default)
        {
            await Context.Set<TEntity>().AddRangeAsync(Entities, cancellationToken);
        }

        public void Update(TEntity entity)
        {
            Context.Set<TEntity>().Update(entity);
        }

        public void UpdateRange(IList<TEntity> entities)
        {
            if (entities == null || entities.Count() == 0) return;
            Context.Set<TEntity>().UpdateRange(entities);
        }

        public void Delete(TEntity Entity)
        {
            Context.Set<TEntity>().Remove(Entity);
        }

        public void Delete(Expression<Func<TEntity, bool>> selector)
        {
            var set = Context.Set<TEntity>();
            var entity = set.FirstOrDefault(selector);
            if (entity != null)
            {
                set.Remove(entity);
            }
        }

        public void DeleteRange(IList<TEntity> entities)
        {
            var set = Context.Set<TEntity>();

            if (entities == null || entities.Count() == 0)
                return;

            set.RemoveRange(entities);
        }

        public void DeleteRange(Expression<Func<TEntity, bool>> selector)
        {
            var set = Context.Set<TEntity>();

            var res = set.Where(selector);

            if (res.Count() == 0) return;

            set.RemoveRange(res);
        }

        #endregion

        #region [ IDbQueryRepository ]

        public TEntity GetEntity(Expression<Func<TEntity, bool>> selector = null)
        {
            var query = Context.Set<TEntity>().AsQueryable();
            if (selector != null)
                return query.FirstOrDefault(selector);
            else
                return query.FirstOrDefault();
        }

        public async Task<TEntity> GetEntityAsync(Expression<Func<TEntity, bool>> selector = null)
        {
            var query = Context.Set<TEntity>().AsQueryable();
            if (selector != null)
                return await query.FirstOrDefaultAsync(selector);
            else
                return await query.FirstOrDefaultAsync();
        }

        public async Task<TEntity> GetEntityAsync(Expression<Func<TEntity, bool>> selector = null, CancellationToken cancellationToken = default)
        {
            var query = Context.Set<TEntity>().AsQueryable();
            if (selector != null)
                return await query.FirstOrDefaultAsync(selector, cancellationToken);
            else
                return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public TResult GetEntity<TResult>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector = null)
        {
            var query = Context.Set<TEntity>().AsQueryable();
            if (selector != null)
                query = query.Where(selector);
            return query
                .Select(converter)
                .FirstOrDefault();
        }

        public async Task<TResult> GetEntityAsync<TResult>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector = null)
        {
            var query = Context.Set<TEntity>().AsQueryable();
            if (selector != null)
                query = query.Where(selector);
            return await query
                .Select(converter)
                .FirstOrDefaultAsync();
        }

        public async Task<TResult> GetEntityAsync<TResult>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector = null, CancellationToken cancellationToken = default)
        {
            var query = Context.Set<TEntity>().AsQueryable();
            if (selector != null)
                query = query.Where(selector);
            return await query
                .Select(converter)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public IList<TEntity> GetList(Expression<Func<TEntity, bool>> selector = null)
        {
            var query = Context.Set<TEntity>().AsQueryable();
            if (selector != null)
                query = query.Where(selector);
            return query.ToList();
        }
        public async Task<IList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> selector = null)
        {
            var query = Context.Set<TEntity>().AsQueryable();
            if (selector != null)
                query = query.Where(selector);
            return await query.ToListAsync();
        }

        public async Task<IList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> selector = null, CancellationToken cancellationToken = default)
        {
            var query = Context.Set<TEntity>().AsQueryable();
            if (selector != null)
                query = query.Where(selector);
            return await query.ToListAsync(cancellationToken);
        }

        public IList<TEntity> GetList<TKey>(Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TKey>> orderBy = null)
        {
            var query = Context.Set<TEntity>().AsQueryable();
            if (selector != null)
                query = query.Where(selector);
            if (orderBy != null)
                query = query.OrderBy(orderBy);
            return query.ToList();
        }

        public async Task<IList<TEntity>> GetListAsync<TKey>(Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TKey>> orderBy = null)
        {
            var query = Context.Set<TEntity>().AsQueryable();
            if (selector != null)
                query = query.Where(selector);
            if (orderBy != null)
                query = query.OrderBy(orderBy);
            return await query.ToListAsync();
        }

        public async Task<IList<TEntity>> GetListAsync<TKey>(Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TKey>> orderBy = null, CancellationToken cancellationToken = default)
        {
            var query = Context.Set<TEntity>().AsQueryable();
            if (selector != null)
                query = query.Where(selector);
            if (orderBy != null)
                query = query.OrderBy(orderBy);
            return await query.ToListAsync(cancellationToken);
        }

        public IList<TResult> GetList<TResult>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector = null)
        {
            var query = Context.Set<TEntity>().AsQueryable();
            if (selector != null)
                query = query.Where(selector);
            return query
                .Select(converter)
                .ToList();
        }

        public async Task<IList<TResult>> GetListAsync<TResult>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector = null)
        {
            var query = Context.Set<TEntity>().AsQueryable();
            if (selector != null)
                query = query.Where(selector);
            return await query
                .Select(converter)
                .ToListAsync();
        }

        public async Task<IList<TResult>> GetListAsync<TResult>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector = null, CancellationToken cancellationToken = default)
        {
            var query = Context.Set<TEntity>().AsQueryable();
            if (selector != null)
                query = query.Where(selector);
            return await query
                .Select(converter)
                .ToListAsync(cancellationToken);
        }

        public IList<TEntity> SqlQuery(string sql, params object[] parameters)
        {
            return Context.Set<TEntity>()
                .FromSqlRaw(sql, parameters)
                .ToList();
        }

        public async Task<IList<TEntity>> SqlQueryAsync(
            string sql,
            params object[] parameters)
        {
            return await Context.Set<TEntity>()
                .FromSqlRaw(sql, parameters)
                .ToListAsync();
        }

        public async Task<IList<TEntity>> SqlQueryAsync(
            string sql,
            CancellationToken cancellationToken = default,
            params object[] parameters)
        {
            return await Context.Set<TEntity>()
                .FromSqlRaw(sql, parameters)
                .ToListAsync(cancellationToken);
        }

        public IList<TResult> SqlQuery<TResult>(
            Expression<Func<TEntity, TResult>> converter,
            string sql,
            params object[] parameters)
        {
            return Context.Set<TEntity>()
                .FromSqlRaw(sql, parameters)
                .Select(converter)
                .ToList();
        }

        public async Task<IList<TResult>> SqlQueryAsync<TResult>(
            Expression<Func<TEntity, TResult>> converter,
            string sql,
            params object[] parameters)
        {
            return await Context.Set<TEntity>()
               .FromSqlRaw(sql, parameters)
               .Select(converter)
               .ToListAsync();
        }

        public async Task<IList<TResult>> SqlQueryAsync<TResult>(
            Expression<Func<TEntity, TResult>> converter,
            string sql,
            CancellationToken cancellationToken = default,
            params object[] parameters)
        {
            return await Context.Set<TEntity>()
               .FromSqlRaw(sql, parameters)
               .Select(converter)
               .ToListAsync(cancellationToken);
        }

        public TResult GetResult<TResult>(
            Expression<Func<TEntity, TResult>> converter,
            Expression<Func<TEntity, bool>> selector)
        {
            return Context.Set<TEntity>()
                .Where(selector)
                .Select(converter)
                .FirstOrDefault();
        }

        public async Task<TResult> GetResultAsync<TResult>(
            Expression<Func<TEntity, TResult>> converter,
            Expression<Func<TEntity, bool>> selector)
        {
            return await Context.Set<TEntity>()
                .Where(selector)
                .Select(converter)
                .FirstOrDefaultAsync();
        }

        public async Task<TResult> GetResultAsync<TResult>(
            Expression<Func<TEntity, TResult>> converter,
            Expression<Func<TEntity, bool>> selector,
            CancellationToken cancellationToken = default)
        {
            return await Context.Set<TEntity>()
                .Where(selector)
                .Select(converter)
                .FirstOrDefaultAsync(cancellationToken);
        }


        public PagedList<TEntity> GetPagedList<TKey>(
            Expression<Func<TEntity, bool>> selector,
            Expression<Func<TEntity, TKey>> orderBy = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var query = Context.Set<TEntity>().AsQueryable();
            if (selector != null)
                query = query.Where(selector);
            if (orderBy != null)
                query = query.OrderBy(orderBy);
            return PagedList<TEntity>.Create(query, pageNumber, pageSize);
        }

        public async Task<PagedList<TEntity>> GetPagedListAsync<TKey>(
            Expression<Func<TEntity, bool>> selector,
            Expression<Func<TEntity, TKey>> orderBy = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var query = Context.Set<TEntity>().AsQueryable();
            if (selector != null)
                query = query.Where(selector);
            if (orderBy != null)
                query = query.OrderBy(orderBy);
            return await PagedList<TEntity>.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task<PagedList<TEntity>> GetPagedListAsync<TKey>(
            Expression<Func<TEntity, bool>> selector,
            Expression<Func<TEntity, TKey>> orderBy = null,
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var query = Context.Set<TEntity>().AsQueryable();
            if (selector != null)
                query = query.Where(selector);
            if (orderBy != null)
                query = query.OrderBy(orderBy);
            return await PagedList<TEntity>.CreateAsync(query, pageNumber, pageSize, cancellationToken);
        }

        public PagedList<TResult> GetPagedList<TKey, TResult>(
            Expression<Func<TEntity, TResult>> converter,
            Expression<Func<TEntity, bool>> selector,
            Expression<Func<TEntity, TKey>> orderBy = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var query = Context.Set<TEntity>().AsQueryable();
            if (selector != null)
                query = query.Where(selector);
            if (orderBy != null)
                query = query.OrderBy(orderBy);
            return PagedList<TResult>.Create(query.Select(converter), pageNumber, pageSize);
        }

        public async Task<PagedList<TResult>> GetPagedListAsync<TKey, TResult>(
            Expression<Func<TEntity, TResult>> converter,
            Expression<Func<TEntity, bool>> selector,
            Expression<Func<TEntity, TKey>> orderBy = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var query = Context.Set<TEntity>().AsQueryable();
            if (selector != null)
                query = query.Where(selector);
            if (orderBy != null)
                query = query.OrderBy(orderBy);
            return await PagedList<TResult>.CreateAsync(query.Select(converter), pageNumber, pageSize);
        }

        public async Task<PagedList<TResult>> GetPagedListAsync<TKey, TResult>(
            Expression<Func<TEntity, TResult>> converter,
            Expression<Func<TEntity, bool>> selector,
            Expression<Func<TEntity, TKey>> orderBy = null,
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var query = Context.Set<TEntity>().AsQueryable();
            if (selector != null)
                query = query.Where(selector);
            if (orderBy != null)
                query = query.OrderBy(orderBy);
            return await PagedList<TResult>.CreateAsync(query.Select(converter), pageNumber, pageSize, cancellationToken);
        }

        #endregion
    }
}