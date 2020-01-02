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
    public partial class DbRepository<TEntity> : IDbQueryRepository<TEntity>, IDbChangeRepository<TEntity>, IDbSaveRepository<TEntity> where TEntity : class
    {
        public DbContext Context { set; get; }
        public DbSet<TEntity> Table { get; set; }
        public IQueryable<TEntity> Query { get; set; }

        public DbRepository(DbContext dbContext)
        {
            Context = dbContext;

            Table = dbContext.Set<TEntity>();

            Query = Table.AsNoTracking().AsQueryable();
        }

        public bool Any(Expression<Func<TEntity, bool>> filter = null)
        {
            var query = Query;
            if (filter != null)
                query = query.Where(filter);

            return query.Any();
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            var query = Query;
            if (filter != null)
                query = query.Where(filter);

            return await query.AnyAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter = null, CancellationToken cancellationToken = default)
        {
            var query = Query;
            if (filter != null)
                query = query.Where(filter);

            return await query.AnyAsync(cancellationToken);
        }

        public int Count(Expression<Func<TEntity, bool>> filter = null)
        {
            var query = Query;
            if (filter != null)
                query = query.Where(filter);

            return query.Count();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            var query = Query;
            if (filter != null)
                query = query.Where(filter);

            return await query.CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null, CancellationToken cancellationToken = default)
        {
            var query = Query;
            if (filter != null)
                query = query.Where(filter);

            return await query.CountAsync(cancellationToken);
        }
    }
}