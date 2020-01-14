﻿namespace Nigel.Data.DbRepositories
{
    public interface IDbRepository<TEntity> : IDbQueryRepository<TEntity>, IDbChangeRepository<TEntity>, IDbSaveRepository<TEntity> where TEntity : class
    {
    }
}