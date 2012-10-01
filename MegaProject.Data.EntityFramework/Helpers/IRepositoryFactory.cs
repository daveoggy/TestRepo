using MegaProject.Data.Contracts;

namespace MegaProject.Data.EntityFramework.Helpers
{
    public interface IRepositoryFactory
    {
        IRepository<T> Create<T>(IEFUnitOfWork uow) where T : class;
        IRepositoryBase Create();
        IRepositoryBase Create(IEFUnitOfWork uow);

    }
}