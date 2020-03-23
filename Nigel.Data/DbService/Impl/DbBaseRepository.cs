using Microsoft.EntityFrameworkCore;
using Nigel.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
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

        public virtual async Task<int> InsertAsync(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentException($"{typeof(T)} is Null");
                }

                await Entities.AddAsync(entity);
                //执行存储
                return _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return -1;
            }
        }

        public virtual async Task<int> BatchInsertAsync(params T[] entities)
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

                //Entities.AddRange(entities);
                //执行存储操作
                return _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return -1;
            }
        }

        public virtual int Update(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentException($"{typeof(T)} is Null");
                }

                Entities.Update(entity);
                //执行存储
                return _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return -1;
            }
        }

        public virtual int BatchUpdate(params T[] entities)
        {
            try
            {
                if (entities == null)
                {
                    throw new ArgumentException($"{typeof(T)} is Null");
                }

                Entities.UpdateRange(entities);
                //执行存储
                return _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return -1;
            }
        }

        public virtual int Delete(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentException($"{typeof(T)} is Null");
                }

                Entities.Remove(entity);
                //执行存储
                return _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return -1;
            }
        }

        public virtual int BatchDelete(params T[] entities)
        {
            try
            {
                if (entities == null)
                {
                    throw new ArgumentException($"{typeof(T)} is Null");
                }

                Entities.RemoveRange(entities);
                //执行存储
                return _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return -1;
            }
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await this.Entities.FindAsync(id);
        }

        public virtual Task<List<T>> GetListAsync()
        {
            return Entities.AsNoTracking().ToListAsync();
        }

        public virtual Task<List<T>> GetListAsync(string orderby)
        {
            return Entities.OrderByBatch(orderby).AsNoTracking().ToListAsync();
        }

        public virtual Task<List<T>> GetListAsync(Expression<Func<T, bool>> selector)
        {
            return Entities.Where(selector).AsNoTracking().ToListAsync();
        }

        public virtual Task<List<T>> GetListAsync(Expression<Func<T, bool>> selector, string orderby)
        {
            return Entities.Where(selector).OrderByBatch(orderby).AsNoTracking().ToListAsync();
        }

        public virtual Task<List<T>> GetListAsync(Expression<Func<T, bool>> selector, int skip = 0, int limit = 20)
        {
            return Entities.Where(selector).Skip(skip).Take(limit).AsNoTracking().ToListAsync();
        }

        public virtual Task<List<T>> GetListAsync(Expression<Func<T, bool>> selector, string orderby, int skip = 0, int limit = 20)
        {
            return Entities.Where(selector).OrderByBatch(orderby).Skip(skip).Take(limit).AsNoTracking().ToListAsync();
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
