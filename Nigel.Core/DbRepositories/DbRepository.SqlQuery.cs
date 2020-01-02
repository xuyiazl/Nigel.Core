﻿using System;
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
        public IList<TEntity> SqlQuery(
            string sql,
            params object[] parameters)
        {
            return Table
                .FromSqlRaw(sql, parameters)
                .ToList();
        }

        public async Task<IList<TEntity>> SqlQueryAsync(
            string sql,
            params object[] parameters)
        {
            return await Table
                .FromSqlRaw(sql, parameters)
                .ToListAsync();
        }

        public async Task<IList<TEntity>> SqlQueryAsync(
            string sql,
            CancellationToken cancellationToken = default,
            params object[] parameters)
        {
            return await Table
                .FromSqlRaw(sql, parameters)
                .ToListAsync(cancellationToken);
        }

        public IList<TResult> SqlQuery<TResult>(
            Expression<Func<TEntity, TResult>> converter,
            string sql,
            params object[] parameters)
        {
            return Table
                .FromSqlRaw(sql, parameters)
                .Select(converter)
                .ToList();
        }

        public async Task<IList<TResult>> SqlQueryAsync<TResult>(
            Expression<Func<TEntity, TResult>> converter,
            string sql,
            params object[] parameters)
        {
            return await Table
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
            return await Table
               .FromSqlRaw(sql, parameters)
               .Select(converter)
               .ToListAsync(cancellationToken);
        }
    }
}