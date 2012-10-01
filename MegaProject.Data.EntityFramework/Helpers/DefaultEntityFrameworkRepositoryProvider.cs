using System;
using System.Collections.Generic;
using System.Data.Entity;
using MegaProject.Data.Contracts;

namespace MegaProject.Data.EntityFramework.Helpers
{
    public class DefaultEntityFrameworkRepositoryProvider : IRepositoryProvider
    {
        private RepositoryFactories _repositoryFactories;
        protected Dictionary<Type, object> Repositories { get; private set; }

        public DbContext Context { get; set; }

        public DefaultEntityFrameworkRepositoryProvider(RepositoryFactories repositoryFactories)
        {
            _repositoryFactories = repositoryFactories;
            Repositories = new Dictionary<Type, object>();
        }

        public virtual IRepository<TEntity> GetRepositoryForEntityType<TEntity>() where TEntity : class
        {
            return GetRepository<IRepository<TEntity>>(_repositoryFactories.GetRepositoryFactoryForEntityType<TEntity>());
        }

        public virtual TRepo GetRepository<TRepo>(Func<EntityFrameworkUnitOfWork, object> factory = null) where TRepo : class
        {
            object repoObj;
            Repositories.TryGetValue(typeof(TRepo), out repoObj);
            if (repoObj != null)
            {
                return (TRepo)repoObj;
            }

            // Not found or null; make one, add to dictionary cache, and return it.
            return MakeRepository<TRepo>(factory, Context);
        }

        protected virtual TRepo MakeRepository<TRepo>(Func<DbContext, object> factory, DbContext dbContext)
        {
            var f = factory ?? _repositoryFactories.GetRepositoryFactory<TRepo>();
            if (f == null)
            {
                throw new NotImplementedException("No factory for repository type, " + typeof(TRepo).FullName);
            }
            var repo = (TRepo)f(dbContext);
            Repositories[typeof(TRepo)] = repo;
            return repo;
        }

        public void SetRepository<TRepo>(TRepo repository)
        {
            Repositories[typeof(TRepo)] = repository;
        }
    }
}
