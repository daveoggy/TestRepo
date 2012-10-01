using System;
using System.Data.Entity;
using MegaProject.Data.Contracts;

namespace MegaProject.Data.EntityFramework.Helpers
{
    // This depends on EF so defined in this assembly
    public interface IRepositoryProvider
    {
        EntityFrameworkUnitOfWork Context { get; set; }

        IRepository<TEntity> GetRepositoryForEntityType<TEntity>() where TEntity : class;

        TRepo GetRepository<TRepo>(Func<EntityFrameworkUnitOfWork, object> factory = null) where TRepo : class;

        void SetRepository<TRepo>(TRepo repository);
    }
}
