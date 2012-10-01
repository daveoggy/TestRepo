using System;
using MegaProject.Data.Contracts;

namespace MegaProject.Data.EntityFramework.Helpers
{
    public class RepositoryFactory : IRepositoryFactory
    {

        public Func<EntityFrameworkUnitOfWork, IRepository<T>> DefaultEntityRepositoryFactory<T>() where T : class
        {
            return uow => new EntityFrameworkRepository<T>(uow);
        }

        public IRepository<T> Create<T>(IEFUnitOfWork uow) where T : class
        {
            return new EntityFrameworkRepository<T>(uow);
        }

        public IRepositoryBase Create()
        {
            return new Repository(new MegaProjectContext());
        }

        public IRepositoryBase Create(IEFUnitOfWork uow)
        {
            return new Repository(uow);
        }

    }
}
