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

        public TEntity GetEntity(Expression<Func<TEntity, bool>> selector = null)
        {
            var query = Query;
            if (selector != null)
                return query.FirstOrDefault(selector);
            else
                return query.FirstOrDefault();
        }

        public TEntity GetEntity<TOrder>(
            Expression<Func<TEntity, bool>> selector = null,
            Expression<Func<TEntity, TOrder>> orderBy = null,
            bool orderDesc = false)
        {
            var query = Query;
            if (selector != null)
                query = query.Where(selector);
            if (orderBy != null)
                query = orderDesc ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
            return query.FirstOrDefault();
        }

        public async Task<TEntity> GetEntityAsync(Expression<Func<TEntity, bool>> selector = null)
        {
            var query = Query;
            if (selector != null)
                return await query.FirstOrDefaultAsync(selector);
            else
                return await query.FirstOrDefaultAsync();
        }

        public async Task<TEntity> GetEntityAsync<TOrder>(Expression<Func<TEntity, bool>> selector = null,
            Expression<Func<TEntity, TOrder>> orderBy = null,
            bool orderDesc = false)
        {
            var query = Query;
            if (selector != null)
                query = query.Where(selector);
            if (orderBy != null)
                query = orderDesc ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<TEntity> GetEntityAsync(Expression<Func<TEntity, bool>> selector = null, CancellationToken cancellationToken = default)
        {
            var query = Query;
            if (selector != null)
                return await query.FirstOrDefaultAsync(selector, cancellationToken);
            else
                return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<TEntity> GetEntityAsync<TOrder>(Expression<Func<TEntity, bool>> selector = null,
            Expression<Func<TEntity, TOrder>> orderBy = null,
            bool orderDesc = false,
            CancellationToken cancellationToken = default)
        {
            var query = Query;
            if (orderBy != null)
                query = orderDesc ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
            if (selector != null)
                query = query.Where(selector);
            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public TResult GetEntity<TResult>(
            Expression<Func<TEntity, TResult>> converter,
            Expression<Func<TEntity, bool>> selector = null)
        {
            var query = Query;
            if (selector != null)
                query = query.Where(selector);
            return query
                .Select(converter)
                .FirstOrDefault();
        }

        public TResult GetEntity<TResult, TOrder>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector = null,
            Expression<Func<TEntity, TOrder>> orderBy = null,
            bool orderDesc = false)
        {
            var query = Query;
            if (selector != null)
                query = query.Where(selector);
            if (orderBy != null)
                query = orderDesc ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
            return query
                .Select(converter)
                .FirstOrDefault();
        }

        public async Task<TResult> GetEntityAsync<TResult>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector = null)
        {
            var query = Query;
            if (selector != null)
                query = query.Where(selector);
            return await query
                .Select(converter)
                .FirstOrDefaultAsync();
        }

        public async Task<TResult> GetEntityAsync<TResult, TOrder>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector = null,
            Expression<Func<TEntity, TOrder>> orderBy = null,
            bool orderDesc = false)
        {
            var query = Query;
            if (selector != null)
                query = query.Where(selector);
            if (orderBy != null)
                query = orderDesc ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
            return await query
                .Select(converter)
                .FirstOrDefaultAsync();
        }

        public async Task<TResult> GetEntityAsync<TResult>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector = null, CancellationToken cancellationToken = default)
        {
            var query = Query;
            if (selector != null)
                query = query.Where(selector);
            return await query
                .Select(converter)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<TResult> GetEntityAsync<TResult, TOrder>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector = null,
            Expression<Func<TEntity, TOrder>> orderBy = null,
            bool orderDesc = false,
            CancellationToken cancellationToken = default)
        {
            var query = Query;
            if (selector != null)
                query = query.Where(selector);
            if (orderBy != null)
                query = orderDesc ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
            return await query
                .Select(converter)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}