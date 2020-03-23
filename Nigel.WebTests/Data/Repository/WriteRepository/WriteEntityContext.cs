using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nigel.WebTests.Data.Repository.WriteRepository
{
    /// <summary>
    /// 只写上下文
    /// </summary>
    public class WriteEntityContext : BaseRepositoryFactory, IWriteEntityContext
    {
        public WriteEntityContext(DbContextOptions<WriteEntityContext> options) : base(options, $"Nigel.WebTests.Data.Mapping")
        {

        }
    }
}
