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
    /// 数据库领域操作的基础对象
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class DbServiceBaseProvider<TEntity> where TEntity : class, new()
    {
        public IBaseRepository<TEntity> readRepository { get; set; }

        public IBaseRepository<TEntity> writeRepository { get; set; }

        protected DbServiceBaseProvider(IBaseRepository<TEntity> readRepository, IBaseRepository<TEntity> writeRepository)
        {
            this.readRepository = readRepository;
            this.writeRepository = writeRepository;
        }

        #region 抽象对象来实现IDbServiceBase中的方法，提供重写操作


        public virtual int Insert(TEntity entity)
        {
            if (writeRepository != null)
            {
                return writeRepository.Insert(entity);
            }

            return -1;
        }

        public virtual async Task<int> InsertAsync(TEntity entity)
        {
            if (writeRepository != null)
            {
                return await writeRepository.InsertAsync(entity);
            }

            return -1;
        }

        public virtual int BatchInsert(TEntity[] entity)
        {
            if (writeRepository != null)
            {
                return writeRepository.BatchInsert(entity);
            }

            return -1;
        }

        public virtual async Task<int> BatchInsertAsync(TEntity[] entity)
        {
            if (writeRepository != null)
            {
                return await writeRepository.BatchInsertAsync(entity);
            }

            return -1;
        }

        public virtual int Update(TEntity entity)
        {
            if (writeRepository != null)
            {
                return writeRepository.Update(entity);
            }

            return -1;
        }

        public virtual int BatchUpdate(TEntity entity)
        {
            if (writeRepository != null)
            {
                return writeRepository.BatchUpdate(entity);
            }

            return -1;
        }

        public virtual int Delete(TEntity entity)
        {
            if (writeRepository != null)
            {
                return writeRepository.Delete(entity);
            }

            return -1;
        }

        public virtual int BatchDelete(TEntity entity)
        {
            if (writeRepository != null)
            {
                return writeRepository.BatchDelete(entity);
            }

            return -1;
        }

        public virtual async Task<TEntity> GetByIdAsync(object id)
        {
            if (readRepository != null)
            {
                return await readRepository.GetByIdAsync(id);
            }

            return default;
        }

        public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> selector)
        {
            if (readRepository != null)
            {
                return await readRepository.GetListAsync(selector);
            }

            return new List<TEntity>();
        }

        public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> selector, string orderby)
        {
            if (readRepository != null)
            {
                return await readRepository.GetListAsync(selector, orderby);
            }

            return new List<TEntity>();
        }

        public virtual async Task<List<TEntity>> GetListAsync()
        {
            if (readRepository != null)
            {
                return await readRepository.GetListAsync();
            }

            return new List<TEntity>();
        }

        public virtual async Task<List<TEntity>> GetListAsync(string orderby)
        {
            if (readRepository != null)
            {
                return await readRepository.GetListAsync(orderby);
            }

            return new List<TEntity>();
        }

        public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> selector, int skip = 0, int limit = 20)
        {
            if (readRepository != null)
            {
                return await readRepository.GetListAsync(selector, skip, limit);
            }

            return new List<TEntity>();
        }
        public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> selector, string orderby, int skip = 0, int limit = 20)
        {
            if (readRepository != null)
            {
                return await readRepository.GetListAsync(selector, orderby, skip, limit);
            }

            return new List<TEntity>();
        }

        public virtual async Task<PagedSkipModel<TEntity>> GetPagedSkipListAsync(Expression<Func<TEntity, bool>> selector, string orderby, int skip = 0, int limit = 20)
        {
            if (readRepository != null)
            {
                return await readRepository.GetPagedSkipListAsync(selector, orderby, skip, limit);
            }

            return new PagedSkipModel<TEntity>(new List<TEntity>(), 0, skip, limit);
        }

        public virtual async Task<PagedModel<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>> selector, string orderby, int pageNumber = 1, int pageSize = 20)
        {
            if (readRepository != null)
            {
                return await readRepository.GetPagedListAsync(selector, orderby, pageNumber, pageSize);
            }

            return new PagedModel<TEntity>(new List<TEntity>(), 0, pageNumber, pageSize);
        }

        public virtual async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> selector)
        {
            if (readRepository != null)
            {
                return await readRepository.GetCountAsync(selector);
            }

            return 0;
        }

        #endregion


        #region 实现Dispose的方法

        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~ServiceBaseProvider() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }

        #endregion
    }
}
