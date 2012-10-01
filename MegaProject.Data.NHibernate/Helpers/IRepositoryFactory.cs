using MegaProject.Data.Contracts;

namespace MegaProject.Data.NHibernate.Helpers
{
    public interface IRepositoryFactory
    {
        IRepositoryBase Create();
        IRepositoryBase Create(INHUnitOfWork uow);
    }
}