using System;
using MegaProject.Data.Contracts;

namespace MegaProject.Data.EntityFramework.Helpers
{
    public static class RepositoryFactories
    {
        //private static readonly IDictionary<Type, Func<EntityFrameworkUnitOfWork, IRepository<T>>> _repositoryFactories;
        //private static object _locker;

        //static RepositoryFactories()
        //{
        //    _repositoryFactories = new Dictionary<Type, Func<EntityFrameworkUnitOfWork, IRepository<T>>>();
        //    _locker = new object();
        //}

        //public RepositoryFactories(IDictionary<Type, Func<EntityFrameworkUnitOfWork, object>> repositoryFactories)
        //{
        //    _repositoryFactories = repositoryFactories;
        //}

        //public static Func<EntityFrameworkUnitOfWork, IRepository<T>> GetRepositoryFactory<T>()
        //{
        //    Func<EntityFrameworkUnitOfWork, IRepository<T>> factory;
        //    lock(_locker)
        //        _repositoryFactories.TryGetValue(typeof(T), out factory);
        //    return factory;
        //}

        public static Func<EntityFrameworkUnitOfWork, IRepository<T>> DefaultEntityRepositoryFactory<T>() where T : class
        {
            return uow => new EntityFrameworkRepository<T>(uow);
        }

        public static IRepository<T> Create<T>(EntityFrameworkUnitOfWork uow) where T : class
        {
            return new EntityFrameworkRepository<T>(uow);
        }

        //public static Func<EntityFrameworkUnitOfWork, object> GetRepositoryFactoryForEntityType<T>() where T : class
        //{
        //    return GetRepositoryFactory<T>() ?? DefaultEntityRepositoryFactory<T>();
        //}
    }
}
