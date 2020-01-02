using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Nigel.Core.Collection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nigel.Core.DbRepositories
{

    public interface IDbChangeRepository<TEntity> where TEntity : class
    {

        DbContext Context { get; }
        DbSet<TEntity> Table { get; }
        DatabaseFacade Database { get; }
        bool IsNoTracking { get; set; }
        void Add(TEntity entity);
        void AddAsync(TEntity entity);
        void AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        void AddRange(IList<TEntity> Entities);
        void AddRangeAsync(IList<TEntity> Entities);
        void AddRangeAsync(IList<TEntity> Entities, CancellationToken cancellationToken = default);
        void Update(TEntity entity);
        void UpdateRange(IList<TEntity> entities);
        void Delete(Expression<Func<TEntity, bool>> selector);
        void Delete(TEntity Entity);
        void DeleteRange(Expression<Func<TEntity, bool>> selector);
        void DeleteRange(IList<TEntity> entities);
    }
}
