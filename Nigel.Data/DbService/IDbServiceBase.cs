﻿using Nigel.Paging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nigel.Data.DbService
{

    /// <summary>
    /// 数据领域层接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IDbServiceBase<TEntity> : IDisposable where TEntity : class, new()
    {

        #region 抽象对象来实现IDbServiceBase中的方法，提供重写操作

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        int Insert(TEntity entity, bool isSaveChange = true);
        Task<int> InsertAsync(TEntity entity, bool isSaveChange = true, CancellationToken cancellationToken = default);
        int BatchInsert(TEntity[] entities, bool isSaveChange = true);
        Task<int> BatchInsertAsync(TEntity[] entities, bool isSaveChange = true, CancellationToken cancellationToken = default);
        int Update(TEntity entity, bool isSaveChange = true);
        Task<int> UpdateAsync(TEntity entity, bool isSaveChange = true, CancellationToken cancellationToken = default);
        int BatchUpdate(TEntity[] entities, bool isSaveChange = true);
        Task<int> BatchUpdateAsync(TEntity[] entities, bool isSaveChange = true, CancellationToken cancellationToken = default);
        int Delete(TEntity entity, bool isSaveChange = true);
        Task<int> DeleteAsync(TEntity entity, bool isSaveChange = true, CancellationToken cancellationToken = default);
        int BatchDelete(TEntity[] entities, bool isSaveChange = true);
        Task<int> BatchDeleteAsync(TEntity[] entities, bool isSaveChange = true, CancellationToken cancellationToken = default);



        TEntity GetById(object id);
        Task<TEntity> GetByIdAsync(object id, CancellationToken cancellationToken = default);
        List<TEntity> GetList();
        Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default);
        List<TEntity> GetList(string orderby);
        Task<List<TEntity>> GetListAsync(string orderby, CancellationToken cancellationToken = default);
        List<TEntity> GetList(Expression<Func<TEntity, bool>> selector);
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> selector, CancellationToken cancellationToken = default);
        List<TEntity> GetList(Expression<Func<TEntity, bool>> selector, string orderby);
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> selector, string orderby, CancellationToken cancellationToken = default);
        List<TEntity> GetList(Expression<Func<TEntity, bool>> selector, int skip = 0, int limit = 20);
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> selector, int skip = 0, int limit = 20, CancellationToken cancellationToken = default);
        List<TEntity> GetList(Expression<Func<TEntity, bool>> selector, string orderby, int skip = 0, int limit = 20);
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> selector, string orderby, int skip = 0, int limit = 20, CancellationToken cancellationToken = default);
        PagedSkipModel<TEntity> GetPagedSkipList(Expression<Func<TEntity, bool>> selector, string orderby, int skip = 0, int limit = 20);
        Task<PagedSkipModel<TEntity>> GetPagedSkipListAsync(Expression<Func<TEntity, bool>> selector, string orderby, int skip = 0, int limit = 20, CancellationToken cancellationToken = default);
        PagedModel<TEntity> GetPagedList(Expression<Func<TEntity, bool>> selector, string orderby, int pageNumber = 1, int pageSize = 20);
        Task<PagedModel<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>> selector, string orderby, int pageNumber = 1, int pageSize = 20, CancellationToken cancellationToken = default);
        bool Any(Expression<Func<TEntity, bool>> selector);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> selector, CancellationToken cancellationToken = default);
        int GetCount(Expression<Func<TEntity, bool>> selector);
        Task<int> GetCountAsync(Expression<Func<TEntity, bool>> selector, CancellationToken cancellationToken = default);

        #endregion

        #region 增加bulkextensions拓展

        int BatchUpdate(Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TEntity>> Update);

        Task<int> BatchUpdateAsync(Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TEntity>> Update, CancellationToken cancellationToken = default);

        int BatchDelete(Expression<Func<TEntity, bool>> selector);

        Task<int> BatchDeleteAsync(Expression<Func<TEntity, bool>> selector, CancellationToken cancellationToken = default);

        #endregion
    }
}
