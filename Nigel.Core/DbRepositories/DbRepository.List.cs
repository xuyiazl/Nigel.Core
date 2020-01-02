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
        public IList<TEntity> GetList(Expression<Func<TEntity, bool>> selector = null)
        {
            var query = Query;
            if (selector != null)
                query = query.Where(selector);
            return query.ToList();
        }

        public async Task<IList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> selector = null)
        {
            var query = Query;
            if (selector != null)
                query = query.Where(selector);
            return await query.ToListAsync();
        }

        public async Task<IList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> selector = null, CancellationToken cancellationToken = default)
        {
            var query = Query;
            if (selector != null)
                query = query.Where(selector);
            return await query.ToListAsync(cancellationToken);
        }

        public IList<TEntity> GetList<TOrder>(
            Expression<Func<TEntity, bool>> selector,
            Expression<Func<TEntity, TOrder>> orderBy = null,
            bool orderDesc = false)
        {
            var query = Query;
            if (selector != null)
                query = query.Where(selector);
            if (orderBy != null)
                query = orderDesc ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
            return query.ToList();
        }

        public async Task<IList<TEntity>> GetListAsync<TOrder>(
            Expression<Func<TEntity, bool>> selector,
            Expression<Func<TEntity, TOrder>> orderBy = null,
            bool orderDesc = false)
        {
            var query = Query;
            if (selector != null)
                query = query.Where(selector);
            if (orderBy != null)
                query = orderDesc ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
            return await query.ToListAsync();
        }

        public async Task<IList<TEntity>> GetListAsync<TOrder>(
            Expression<Func<TEntity, bool>> selector,
            Expression<Func<TEntity, TOrder>> orderBy = null,
            bool orderDesc = false,
            CancellationToken cancellationToken = default)
        {
            var query = Query;
            if (selector != null)
                query = query.Where(selector);
            if (orderBy != null)
                query = orderDesc ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
            return await query.ToListAsync(cancellationToken);
        }

        public IList<TResult> GetList<TResult>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector = null)
        {
            var query = Query;
            if (selector != null)
                query = query.Where(selector);
            return query
                .Select(converter)
                .ToList();
        }

        public async Task<IList<TResult>> GetListAsync<TResult>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector = null)
        {
            var query = Query;
            if (selector != null)
                query = query.Where(selector);
            return await query
                .Select(converter)
                .ToListAsync();
        }

        public async Task<IList<TResult>> GetListAsync<TResult>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector = null, CancellationToken cancellationToken = default)
        {
            var query = Query;
            if (selector != null)
                query = query.Where(selector);
            return await query
                .Select(converter)
                .ToListAsync(cancellationToken);
        }
    }
}