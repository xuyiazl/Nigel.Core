using Nigel.Data.Collection.Paged;
using Nigel.Paging;
using Nigel.WebTests.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nigel.WebTests.Data.DbService
{
    public interface IDbAdminUsersServiceProvider : IDbDependencyService
    {
        int Delete(AdminUsers entity);
        Task<AdminUsers> GetByIdAsync(long id);
        Task<int> GetCountAsync(Expression<Func<AdminUsers, bool>> selector);
        Task<List<AdminUsers>> GetListAsync(Expression<Func<AdminUsers, bool>> expression, string orderby, int pageIndex = 1, int pageSize = 20);
        Task<List<AdminUsers>> GetListAsync(Expression<Func<AdminUsers, bool>> expression, string orderby, int limit);
        Task<PagedSkipModel<AdminUsers>> GetPagedSkipListAsync(Expression<Func<AdminUsers, bool>> selector, string orderby, int skip = 0, int limit = 20);
        Task<PagedModel<AdminUsers>> GetPagedListAsync(Expression<Func<AdminUsers, bool>> expression, string orderby, int pageNumber = 1, int pageSize = 20);
        Task<int> InsertAsync(AdminUsers entity);
        int Update(AdminUsers entity);
    }
}