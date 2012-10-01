using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MegaProject.Data.Contracts
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        IQueryable<TEntity> All { get; }
        IQueryable<TEntity> AllIncluding(params Expression<Func<TEntity, object>>[] includeProperties);
        TEntity GetById(int id);
        TEntity GetById(System.Guid id);
        void Add(TEntity entity);
        void Add(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
