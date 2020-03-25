using Nigel.Data.DbService;
using Nigel.Paging;
using Nigel.WebTests.Data.Entity;
using Nigel.WebTests.Data.Repository.ReadRepository;
using Nigel.WebTests.Data.Repository.WriteRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nigel.WebTests.Data.DbService
{
    public class DbAdminUsersServiceProvider : DbServiceBaseProvider<AdminUsers>, IDbAdminUsersServiceProvider
    {
        public DbAdminUsersServiceProvider(IReadRepository<AdminUsers> readRepository, IWriteRepository<AdminUsers> writeRepository)
            : base(readRepository, writeRepository)
        {

        }

        public new async Task<int> GetCountAsync(Expression<Func<AdminUsers, bool>> selector)
        {
            return await readRepository.GetCountAsync(selector);
        }

        public Task<List<AdminUsers>> GetListAsync(Expression<Func<AdminUsers, bool>> selector, string orderby, int limit)
        {
            return readRepository.GetListAsync(selector, orderby, 0, limit);
        }

        public new async Task<PagedSkipModel<AdminUsers>> GetPagedSkipListAsync(Expression<Func<AdminUsers, bool>> selector, string orderby, int skip = 0, int limit = 20)
        {
            return await readRepository.GetPagedSkipListAsync(selector, orderby, skip, limit);
        }

        public new async Task<PagedModel<AdminUsers>> GetPagedListAsync(Expression<Func<AdminUsers, bool>> selector, string orderby, int pageIndex = 1, int pageSize = 20)
        {
            return await readRepository.GetPagedListAsync(selector, orderby, pageIndex, pageSize);
        }

        public async Task<AdminUsers> GetByIdAsync(long id)
        {
            return await readRepository.GetByIdAsync(id);
        }

        public new int Insert(AdminUsers entity)
        {
            return writeRepository.Insert(entity);
        }

        public new int Update(AdminUsers entity)
        {
            return writeRepository.Update(entity);
        }

        public new int Delete(AdminUsers entity)
        {
            return writeRepository.Delete(entity);
        }
    }
}
