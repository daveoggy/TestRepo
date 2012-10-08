using MegaProject.Data.Contracts;

namespace MegaProject.Data.EntityFramework.Helpers
{
    public class RepositoryFactory : IRepositoryFactory
    {
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
