using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Nigel.Data.Collection;

namespace Nigel.Data.DbRepositories
{
    public partial class DbRepository<TEntity> : IDbRepository<TEntity> where TEntity : class
    {
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

    }
}