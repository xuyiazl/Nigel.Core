using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Nigel.Data.Collection;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nigel.Data.DbRepositories
{
    public interface IDbSaveRepository<TEntity> where TEntity : class
    {
        DbContext Context { get; }
        DbSet<TEntity> Table { get; }
        DatabaseFacade Database { get; }
        bool IsNoTracking { get; set; }
        bool Save();
        Task<bool> SaveAsync();
        Task<bool> SaveAsync(CancellationToken cancellationToken = default);
        IDbContextTransaction BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
    }
}
