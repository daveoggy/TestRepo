using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MegaProject.Data.Contracts
{
    public interface IRepositoryBase : IDisposable
    {
        IQueryable<TEntity> All<TEntity>() where TEntity : class;
        IQueryable<TEntity> AllIncluding<TEntity>(params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class;
        TEntity GetById<TEntity>(int id) where TEntity : class;
        TEntity GetById<TEntity>(System.Guid id) where TEntity : class;
        void Add<TEntity>(TEntity entity) where TEntity : class;
        void Add<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        void Update<TEntity>(TEntity entity) where TEntity : class;
        void Delete<TEntity>(TEntity entity) where TEntity : class;
        void Save();
    }
}
