using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Nigel.Data.DbService
{

    /// <summary>
    /// 数据领域层接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IDbServiceBase<TEntity> : IDisposable where TEntity : class, new()
    {
        /// <summary>
        /// 执行添加操作
        /// </summary>
        /// <param name="entity"></param>
        int Insert(TEntity entity);
        /// <summary>
        /// 批量添加操作
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        int InsertBatch(params TEntity[] entities);
        /// <summary>
        /// 执行修改操作
        /// </summary>
        /// <param name="entity"></param>
        int Update(TEntity entity);
        /// <summary>
        /// 移除数据操作
        /// </summary>
        /// <param name="entity"></param>
        int Delete(TEntity entity);
        /// <summary>
        /// 异步返回集合对象
        /// </summary>
        /// <returns></returns>
        Task<List<TEntity>> GetListAsync();
        /// <summary>
        /// 异步返回集合对象
        /// </summary>
        /// <param name="orderby">exp:"name asc,createtime desc"</param>
        /// <returns></returns>
        Task<List<TEntity>> GetListAsync(string orderby);
        /// <summary>
        /// 通过表达式条件返回指定集合对象
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> selector);
        /// <summary>
        /// 通过表达式条件返回指定集合对象
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:"name asc,createtime desc"</param>
        /// <returns></returns>
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> selector, string orderby);
        /// <summary>
        /// 通过表达式条件返回指定集合对象，基于分页操作
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="skip"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> selector, int skip = 0, int limit = 20);
        /// <summary>
        /// 通过表达式条件返回指定集合对象，基于分页操作
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:"name asc,createtime desc"</param>
        /// <param name="skip"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> selector, string orderby, int skip = 0, int limit = 20);
        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        Task<int> GetCountAsync(Expression<Func<TEntity, bool>> selector);
    }
}
