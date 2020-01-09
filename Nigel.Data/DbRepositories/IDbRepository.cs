using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Nigel.Data.Collection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nigel.Data.DbRepositories
{

    public interface IDbRepository<TEntity> : IDbQueryRepository<TEntity>, IDbChangeRepository<TEntity>, IDbSaveRepository<TEntity> where TEntity : class
    {

    }
}
