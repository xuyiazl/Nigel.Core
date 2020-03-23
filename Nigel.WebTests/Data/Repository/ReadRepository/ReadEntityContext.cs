using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nigel.WebTests.Data.Repository.ReadRepository
{
    public class ReadEntityContext : BaseRepositoryFactory, IReadEntityContext
    {
        public ReadEntityContext(DbContextOptions<ReadEntityContext> options) : base(options, $"Nigel.WebTests.Data.Mapping")
        {

        }

    }
}
