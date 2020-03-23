using Nigel.Data.Collection.Paged;
using Nigel.Paging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Nigel.Data.DbService
{

    /// <summary>
    /// 通用仓储库的方法定义
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseRepository<T> where T : class, new()
    {
        int Insert(T entity);

        Task<int> InsertAsync(T entity);

        int BatchInsert(params T[] entities);

        Task<int> BatchInsertAsync(params T[] entities);

        int Update(T entity);

        int BatchUpdate(params T[] entities);

        int Delete(T entity);

        int BatchDelete(params T[] entities);

        Task<T> GetByIdAsync(object id);

        /// <summary>
        /// 异步返回集合对象
        /// </summary>
        /// <returns></returns>
        Task<List<T>> GetListAsync();
        /// <summary>
        /// 异步返回集合对象
        /// </summary>
        /// <param name="orderby">exp:"name asc,createtime desc"</param>
        /// <returns></returns>
        Task<List<T>> GetListAsync(string orderby);
        /// <summary>
        /// 通过表达式条件返回指定集合对象
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        Task<List<T>> GetListAsync(Expression<Func<T, bool>> selector);
        /// <summary>
        /// 通过表达式条件返回指定集合对象
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:"name asc,createtime desc"</param>
        /// <returns></returns>
        Task<List<T>> GetListAsync(Expression<Func<T, bool>> selector, string orderby);
        /// <summary>
        /// 通过表达式条件返回指定集合对象，基于分页操作
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="skip"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        Task<List<T>> GetListAsync(Expression<Func<T, bool>> selector, int skip = 0, int limit = 20);
        /// <summary>
        /// 通过表达式条件返回指定集合对象，基于分页操作
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:"name asc,createtime desc"</param>
        /// <param name="skip"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        Task<List<T>> GetListAsync(Expression<Func<T, bool>> selector, string orderby, int skip = 0, int limit = 20);
        /// <summary>
        /// 通过表达式条件返回指定集合对象，基于分页操作
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:"name asc,createtime desc"</param>
        /// <param name="skip"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        Task<PagedSkipModel<T>> GetPagedSkipListAsync(Expression<Func<T, bool>> selector, string orderby, int skip = 0, int limit = 20);
        /// <summary>
        /// 通过表达式条件返回指定集合对象，基于分页操作
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:"name asc,createtime desc"</param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PagedModel<T>> GetPagedListAsync(Expression<Func<T, bool>> selector, string orderby, int pageNumber = 1, int pageSize = 20);
        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        Task<int> GetCountAsync(Expression<Func<T, bool>> selector);

    }
}
