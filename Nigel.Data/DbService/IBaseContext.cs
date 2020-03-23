using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nigel.Data.DbService
{
    public interface IBaseContext
    {
        string ConnectionStrings { get; set; }
        DbSet<TEntity> Set<TEntity>() where TEntity : class;


        int SaveChanges();
        /// <summary>
        /// 批量插入操作，拓展使用EFCore.BulkExtensions 组件
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        void BatchInsert<TEntity>(IList<TEntity> entities) where TEntity : class, new();
        ///// <summary>
        ///// 执行异步存储操作
        ///// </summary>
        ///// <returns></returns>
        //Task<int> SaveChangeAsync(CancellationToken cancellationToken = default);

        /*IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters)
            where TEntity : BaseEntity, new();*/

        //IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters);

        //int ExecuteSqlCommand(string sql, int? timeout = null, params object[] parameters);
    }
}
