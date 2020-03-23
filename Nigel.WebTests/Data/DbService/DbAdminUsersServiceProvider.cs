using Nigel.Data.Collection.Paged;
using Nigel.Data.DbService;
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

        public Task<List<AdminUsers>> GetListAsync(Expression<Func<AdminUsers, bool>> expression, string orderby, int limit)
        {
            return readRepository.GetListAsync(expression, orderby, 0, limit);
        }

        public new async Task<List<AdminUsers>> GetListAsync(Expression<Func<AdminUsers, bool>> expression, string orderby, int pageIndex = 1, int pageSize = 20)
        {
            return await readRepository.GetListAsync(expression, orderby, (pageIndex - 1) * pageSize, pageSize);
        }

        public async Task<PagedModel<AdminUsers>> GetPagedListAsync(Expression<Func<AdminUsers, bool>> expression, string orderby, int pageIndex = 1, int pageSize = 20)
        {
            var totalRecords = await readRepository.GetCountAsync(expression);

            var list = await readRepository.GetListAsync(expression, orderby, (pageIndex - 1) * pageSize, pageSize);

            return new PagedModel<AdminUsers>(list, totalRecords, pageIndex, pageSize);
        }

        public async Task<AdminUsers> GetByIdAsync(long id)
        {
            return await readRepository.GetByIdAsync(id);
        }

        public override async Task<int> InsertAsync(AdminUsers entity)
        {
            return await writeRepository.InsertAsync(entity);
        }

        public override int Update(AdminUsers entity)
        {
            return writeRepository.Update(entity);
        }

        public override int Delete(AdminUsers entity)
        {
            return writeRepository.Delete(entity);
        }
    }
}
