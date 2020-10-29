using MiCarpeta.Repository.Entities;
using System;
using System.Collections.Generic;

namespace MiCarpeta.Repository
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        bool Add(TEntity obj);
        List<TEntity> GetByList(List<FilterQuery> valuesAtributeScanOperator);
        bool Update(TEntity obj);
    }
}
