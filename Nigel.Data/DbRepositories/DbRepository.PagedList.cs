using Nigel.Data.Collection.Paged;
using Nigel.Extensions;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Nigel.Data.DbRepositories
{
    public partial class DbRepository<TEntity> : IDbRepository<TEntity> where TEntity : class
    {
        public PagedList<TEntity> GetPagedList<TOrder>(
            Expression<Func<TEntity, bool>> selector,
            Expression<Func<TEntity, TOrder>> orderBy = null,
            bool orderDesc = false,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var query = this.AsNoTracking();
            if (selector != null)
                query = query.Where(selector);
            if (orderBy != null)
                query = orderDesc ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
            return PagedList<TEntity>.Create(query, pageNumber, pageSize);
        }

        public PagedList<TEntity> GetPagedList(
            Expression<Func<TEntity, bool>> selector,
            string orderBy = "",
            int pageNumber = 1,
            int pageSize = 10)
        {
            var query = this.AsNoTracking();
            if (selector != null)
                query = query.Where(selector);
            if (!string.IsNullOrEmpty(orderBy))
                query = query.OrderByBatch(orderBy);
            return PagedList<TEntity>.Create(query, pageNumber, pageSize);
        }

        public async Task<PagedList<TEntity>> GetPagedListAsync<TOrder>(
            Expression<Func<TEntity, bool>> selector,
            Expression<Func<TEntity, TOrder>> orderBy = null,
            bool orderDesc = false,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var query = this.AsNoTracking();
            if (selector != null)
                query = query.Where(selector);
            if (orderBy != null)
                query = orderDesc ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
            return await PagedList<TEntity>.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task<PagedList<TEntity>> GetPagedListAsync(
            Expression<Func<TEntity, bool>> selector,
            string orderBy = "",
            int pageNumber = 1,
            int pageSize = 10)
        {
            var query = this.AsNoTracking();
            if (selector != null)
                query = query.Where(selector);
            if (!string.IsNullOrEmpty(orderBy))
                query = query.OrderByBatch(orderBy);
            return await PagedList<TEntity>.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task<PagedList<TEntity>> GetPagedListAsync<TOrder>(
            Expression<Func<TEntity, bool>> selector,
            Expression<Func<TEntity, TOrder>> orderBy = null,
            bool orderDesc = false,
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var query = this.AsNoTracking();
            if (selector != null)
                query = query.Where(selector);
            if (orderBy != null)
                query = orderDesc ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
            return await PagedList<TEntity>.CreateAsync(query, pageNumber, pageSize, cancellationToken);
        }

        public async Task<PagedList<TEntity>> GetPagedListAsync(
            Expression<Func<TEntity, bool>> selector,
            string orderBy = "",
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var query = this.AsNoTracking();
            if (selector != null)
                query = query.Where(selector);
            if (!string.IsNullOrEmpty(orderBy))
                query = query.OrderByBatch(orderBy);
            return await PagedList<TEntity>.CreateAsync(query, pageNumber, pageSize, cancellationToken);
        }

        public PagedList<TResult> GetPagedList<TOrder, TResult>(
            Expression<Func<TEntity, TResult>> converter,
            Expression<Func<TEntity, bool>> selector,
            Expression<Func<TEntity, TOrder>> orderBy = null,
            bool orderDesc = false,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var query = this.AsNoTracking();
            if (selector != null)
                query = query.Where(selector);
            if (orderBy != null)
                query = orderDesc ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
            return PagedList<TResult>.Create(query.Select(converter), pageNumber, pageSize);
        }

        public PagedList<TResult> GetPagedList<TResult>(
            Expression<Func<TEntity, TResult>> converter,
            Expression<Func<TEntity, bool>> selector,
            string orderBy = "",
            int pageNumber = 1,
            int pageSize = 10)
        {
            var query = this.AsNoTracking();
            if (selector != null)
                query = query.Where(selector);
            if (!string.IsNullOrEmpty(orderBy))
                query = query.OrderByBatch(orderBy);
            return PagedList<TResult>.Create(query.Select(converter), pageNumber, pageSize);
        }

        public async Task<PagedList<TResult>> GetPagedListAsync<TOrder, TResult>(
            Expression<Func<TEntity, TResult>> converter,
            Expression<Func<TEntity, bool>> selector,
            Expression<Func<TEntity, TOrder>> orderBy = null,
            bool orderDesc = false,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var query = this.AsNoTracking();
            if (selector != null)
                query = query.Where(selector);
            if (orderBy != null)
                query = orderDesc ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);

            return await PagedList<TResult>.CreateAsync(query.Select(converter), pageNumber, pageSize);
        }

        public async Task<PagedList<TResult>> GetPagedListAsync<TResult>(
            Expression<Func<TEntity, TResult>> converter,
            Expression<Func<TEntity, bool>> selector,
            string orderBy = "",
            int pageNumber = 1,
            int pageSize = 10)
        {
            var query = this.AsNoTracking();
            if (selector != null)
                query = query.Where(selector);
            if (!string.IsNullOrEmpty(orderBy))
                query = query.OrderByBatch(orderBy);

            return await PagedList<TResult>.CreateAsync(query.Select(converter), pageNumber, pageSize);
        }

        public async Task<PagedList<TResult>> GetPagedListAsync<TOrder, TResult>(
            Expression<Func<TEntity, TResult>> converter,
            Expression<Func<TEntity, bool>> selector,
            Expression<Func<TEntity, TOrder>> orderBy = null,
            bool orderDesc = false,
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var query = this.AsNoTracking();
            if (selector != null)
                query = query.Where(selector);
            if (orderBy != null)
                query = orderDesc ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
            return await PagedList<TResult>.CreateAsync(query.Select(converter), pageNumber, pageSize, cancellationToken);
        }

        public async Task<PagedList<TResult>> GetPagedListAsync<TResult>(
            Expression<Func<TEntity, TResult>> converter,
            Expression<Func<TEntity, bool>> selector,
            string orderBy = "",
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var query = this.AsNoTracking();
            if (selector != null)
                query = query.Where(selector);
            if (!string.IsNullOrEmpty(orderBy))
                query = query.OrderByBatch(orderBy);
            return await PagedList<TResult>.CreateAsync(query.Select(converter), pageNumber, pageSize, cancellationToken);
        }
    }
}