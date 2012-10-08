using MegaProject.Data.Contracts;

namespace MegaProject.Data.EntityFramework.Helpers
{
    public interface IRepositoryFactory
    {
        IRepositoryBase Create();
        IRepositoryBase Create(IEFUnitOfWork uow);
    }
}