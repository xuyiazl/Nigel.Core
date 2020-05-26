using Microsoft.EntityFrameworkCore;
using Nigel.Extensions;
using Nigel.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nigel.Data.DbService
{

    /// <summary>
    /// 数据库的基础仓储库
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DbBaseRepository<T> where T : class, new()
    {
        protected string _connectionString { get; set; } = "";
        protected readonly IBaseContext _context;
        protected DbSet<T> _entities { get; set; }
        public DbBaseRepository(IBaseContext context)
        {
            _connectionString = context.ConnectionStrings;
            _context = context;
        }
        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
        public virtual int Insert(T entity, bool isSaveChange = true)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentException($"{typeof(T)} is Null");
                }

                Entities.Add(entity);

                if (isSaveChange)
                    return SaveChanges();
                return 0;
            }
            catch (DbUpdateException)
            {
                return -1;
            }
        }
        public virtual async Task<int> InsertAsync(T entity, bool isSaveChange = true, CancellationToken cancellationToken = default)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentException($"{typeof(T)} is Null");
                }

                await Entities.AddAsync(entity);

                if (isSaveChange)
                    return await SaveChangesAsync(cancellationToken);
                return 0;
            }
            catch (DbUpdateException)
            {
                return -1;
            }
        }
        public virtual int BatchInsert(T[] entities, bool isSaveChange = true)
        {
            try
            {
                if (entities == null)
                {
                    throw new ArgumentException($"{typeof(T)} is Null");
                }

                //自增ID操作会出现问题，暂时无法解决自增操作的方式，只能使用笨办法，通过多次连接数据库的方式执行
                //var changeRecord = 0;
                //foreach (var item in entities)
                //{
                //    var entry = Entities.Add(item);
                //    entry.State = EntityState.Added;
                //    changeRecord += _context.SaveChanges();
                //}

                Entities.AddRange(entities);

                if (isSaveChange)
                    return SaveChanges();
                return 0;
            }
            catch (DbUpdateException)
            {
                return -1;
            }
        }
        public virtual async Task<int> BatchInsertAsync(T[] entities, bool isSaveChange = true, CancellationToken cancellationToken = default)
        {
            try
            {
                if (entities == null)
                {
                    throw new ArgumentException($"{typeof(T)} is Null");
                }

                //自增ID操作会出现问题，暂时无法解决自增操作的方式，只能使用笨办法，通过多次连接数据库的方式执行
                //var changeRecord = 0;
                //foreach (var item in entities)
                //{
                //    var entry = Entities.Add(item);
                //    entry.State = EntityState.Added;
                //    changeRecord += _context.SaveChanges();
                //}

                await Entities.AddRangeAsync(entities);

                if (isSaveChange)
                    return await SaveChangesAsync(cancellationToken);
                return 0;
            }
            catch (DbUpdateException)
            {
                return -1;
            }
        }
        public virtual int Update(T entity, bool isSaveChange = true)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentException($"{typeof(T)} is Null");
                }

                Entities.Update(entity);

                if (isSaveChange)
                    return SaveChanges();
                return 0;
            }
            catch (DbUpdateException)
            {
                return -1;
            }
        }
        public virtual async Task<int> UpdateAsync(T entity, bool isSaveChange = true, CancellationToken cancellationToken = default)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentException($"{typeof(T)} is Null");
                }

                Entities.Update(entity);

                if (isSaveChange)
                    return await SaveChangesAsync(cancellationToken);
                return 0;
            }
            catch (DbUpdateException)
            {
                return -1;
            }
        }
        public virtual int Update(T entity, List<string> updatePropertyList, bool modified = true, bool isSaveChange = true)
        {
            if (entity == null)
            {
                return 0;
            }
            var entry = Entities.Attach(entity);
            //var entry = _context.Entry(entity);
            if (updatePropertyList == null)
            {
                entry.State = EntityState.Modified;//全字段更新
            }
            else
            {
                if (modified)
                {
                    updatePropertyList.ForEach(c =>
                    {
                        entry.Property(c).IsModified = true; //部分字段更新的写法
                    });
                }
                else
                {
                    entry.State = EntityState.Modified;//全字段更新
                    updatePropertyList.ForEach(c =>
                    {
                        entry.Property(c).IsModified = false; //部分字段不更新的写法
                    });
                }
            }
            if (isSaveChange)
                return SaveChanges();
            return 0;
        }
        public virtual async Task<int> UpdateAsync(T entity, List<string> updatePropertyList, bool modified = true, bool isSaveChange = true, CancellationToken cancellationToken = default)
        {
            if (entity == null)
            {
                return 0;
            }
            var entry = Entities.Attach(entity);
            //var entry = _context.Entry(entity);
            if (updatePropertyList == null)
            {
                entry.State = EntityState.Modified;//全字段更新
            }
            else
            {
                if (modified)
                {
                    updatePropertyList.ForEach(c =>
                    {
                        entry.Property(c).IsModified = true; //部分字段更新的写法
                    });
                }
                else
                {
                    entry.State = EntityState.Modified;//全字段更新
                    updatePropertyList.ForEach(c =>
                    {
                        entry.Property(c).IsModified = false; //部分字段不更新的写法
                    });
                }
            }
            if (isSaveChange)
                return await SaveChangesAsync(cancellationToken);
            return 0;
        }
        public virtual int BatchUpdate(T[] entities, bool isSaveChange = true)
        {
            try
            {
                if (entities == null)
                {
                    throw new ArgumentException($"{typeof(T)} is Null");
                }

                Entities.UpdateRange(entities);

                if (isSaveChange)
                    return SaveChanges();
                return 0;
            }
            catch (DbUpdateException)
            {
                return -1;
            }
        }
        public virtual async Task<int> BatchUpdateAsync(T[] entities, bool isSaveChange = true, CancellationToken cancellationToken = default)
        {
            try
            {
                if (entities == null)
                {
                    throw new ArgumentException($"{typeof(T)} is Null");
                }

                Entities.UpdateRange(entities);

                if (isSaveChange)
                    return await SaveChangesAsync(cancellationToken);
                return 0;
            }
            catch (DbUpdateException)
            {
                return -1;
            }
        }
        public virtual int Delete(T entity, bool isSaveChange = true)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentException($"{typeof(T)} is Null");
                }

                Entities.Remove(entity);

                if (isSaveChange)
                    return SaveChanges();
                return 0;
            }
            catch (DbUpdateException)
            {
                return -1;
            }
        }
        public virtual async Task<int> DeleteAsync(T entity, bool isSaveChange = true, CancellationToken cancellationToken = default)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentException($"{typeof(T)} is Null");
                }

                Entities.Remove(entity);

                if (isSaveChange)
                    return await SaveChangesAsync(cancellationToken);
                return 0;
            }
            catch (DbUpdateException)
            {
                return -1;
            }
        }
        public virtual int BatchDelete(T[] entities, bool isSaveChange = true)
        {
            try
            {
                if (entities == null)
                {
                    throw new ArgumentException($"{typeof(T)} is Null");
                }

                Entities.RemoveRange(entities);
                if (isSaveChange)
                    return SaveChanges();
                return 0;
            }
            catch (DbUpdateException)
            {
                return -1;
            }
        }
        public virtual async Task<int> BatchDeleteAsync(T[] entities, bool isSaveChange = true, CancellationToken cancellationToken = default)
        {
            try
            {
                if (entities == null)
                {
                    throw new ArgumentException($"{typeof(T)} is Null");
                }

                Entities.RemoveRange(entities);
                if (isSaveChange)
                    return await SaveChangesAsync(cancellationToken);
                return 0;
            }
            catch (DbUpdateException)
            {
                return -1;
            }
        }

        public T GetById(object id)
        {
            return this.Entities.Find(id);
        }
        public async Task<T> GetByIdAsync(object id)
        {
            return await this.Entities.FindAsync(id);
        }
        public virtual List<T> GetList()
        {
            return Entities.AsNoTracking().ToList();
        }
        public virtual async Task<List<T>> GetListAsync()
        {
            return await Entities.AsNoTracking().ToListAsync();
        }
        public virtual List<T> GetList(string orderby)
        {
            return Entities.OrderByBatch(orderby).AsNoTracking().ToList();
        }
        public virtual async Task<List<T>> GetListAsync(string orderby)
        {
            return await Entities.OrderByBatch(orderby).AsNoTracking().ToListAsync();
        }
        public virtual List<T> GetList(Expression<Func<T, bool>> selector)
        {
            return Entities.Where(selector).AsNoTracking().ToList();
        }
        public virtual async Task<List<T>> GetListAsync(Expression<Func<T, bool>> selector)
        {
            return await Entities.Where(selector).AsNoTracking().ToListAsync();
        }
        public virtual List<T> GetList(Expression<Func<T, bool>> selector, string orderby)
        {
            return Entities.Where(selector).OrderByBatch(orderby).AsNoTracking().ToList();
        }
        public virtual async Task<List<T>> GetListAsync(Expression<Func<T, bool>> selector, string orderby)
        {
            return await Entities.Where(selector).OrderByBatch(orderby).AsNoTracking().ToListAsync();
        }
        public virtual List<T> GetList(Expression<Func<T, bool>> selector, int skip = 0, int limit = 20)
        {
            return Entities.Where(selector).Skip(skip).Take(limit).AsNoTracking().ToList();
        }
        public virtual async Task<List<T>> GetListAsync(Expression<Func<T, bool>> selector, int skip = 0, int limit = 20)
        {
            return await Entities.Where(selector).Skip(skip).Take(limit).AsNoTracking().ToListAsync();
        }
        public virtual List<T> GetList(Expression<Func<T, bool>> selector, string orderby, int skip = 0, int limit = 20)
        {
            return Entities.Where(selector).OrderByBatch(orderby).Skip(skip).Take(limit).AsNoTracking().ToList();
        }
        public virtual async Task<List<T>> GetListAsync(Expression<Func<T, bool>> selector, string orderby, int skip = 0, int limit = 20)
        {
            return await Entities.Where(selector).OrderByBatch(orderby).Skip(skip).Take(limit).AsNoTracking().ToListAsync();
        }
        public virtual PagedSkipModel<T> GetPagedSkipList(Expression<Func<T, bool>> selector, string orderby, int skip = 0, int limit = 20)
        {
            var totalRecords = GetCount(selector);

            var list = GetList(selector, orderby, skip, limit);

            return new PagedSkipModel<T>(list, totalRecords, skip, limit);
        }
        public virtual async Task<PagedSkipModel<T>> GetPagedSkipListAsync(Expression<Func<T, bool>> selector, string orderby, int skip = 0, int limit = 20)
        {
            var totalRecords = await GetCountAsync(selector);

            var list = await GetListAsync(selector, orderby, skip, limit);

            return new PagedSkipModel<T>(list, totalRecords, skip, limit);
        }
        public virtual PagedModel<T> GetPagedList(Expression<Func<T, bool>> selector, string orderby, int pageNumber = 1, int pageSize = 20)
        {
            var totalRecords = GetCount(selector);

            var list = GetList(selector, orderby, (pageNumber - 1) * pageSize, pageSize);

            return new PagedModel<T>(list, totalRecords, pageNumber, pageSize);
        }
        public virtual async Task<PagedModel<T>> GetPagedListAsync(Expression<Func<T, bool>> selector, string orderby, int pageNumber = 1, int pageSize = 20)
        {
            var totalRecords = await GetCountAsync(selector);

            var list = await GetListAsync(selector, orderby, (pageNumber - 1) * pageSize, pageSize);

            return new PagedModel<T>(list, totalRecords, pageNumber, pageSize);
        }
        public virtual bool Any(Expression<Func<T, bool>> selector)
        {
            return Entities.Where(selector).Any();
        }
        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> selector)
        {
            return await Entities.Where(selector).AnyAsync();
        }
        public virtual int GetCount(Expression<Func<T, bool>> selector)
        {
            return Entities.AsNoTracking().Count(selector);
        }
        public virtual Task<int> GetCountAsync(Expression<Func<T, bool>> selector)
        {
            return Entities.AsNoTracking().CountAsync(selector);
        }
        private DbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                {
                    _entities = _context.Set<T>();
                }

                return _entities;
            }
        }
    }
}
