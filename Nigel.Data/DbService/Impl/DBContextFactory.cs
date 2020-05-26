﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nigel.Data.DbService
{
    /// <summary>
    /// 基于db上下文拓展工厂，用于拓展EFCore.BulkExtensions的GitHub开源项目
    /// </summary>
    public abstract class DBContextFactory : DbContext
    {
        /// <summary>
        /// 映射的路径
        /// </summary>
        protected string mappingPath { get; set; }
        protected DBContextFactory(DbContextOptions options, string mappingPath) : base(options)
        {
            this.mappingPath = mappingPath;
        }

        public virtual void BatchInsert<TEntity>(IList<TEntity> entities) where TEntity : class, new()
        {
            base.AddRange(entities);
        }

        public virtual int SaveChanges()
        {
            return base.SaveChanges();
        }

        public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
