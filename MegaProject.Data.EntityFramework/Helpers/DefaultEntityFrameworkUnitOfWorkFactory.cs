using System.Data;
using MegaProject.Data.Contracts;

namespace MegaProject.Data.EntityFramework.Helpers
{
    public class DefaultEntityFrameworkUnitOfWorkFactory : IUnitOfWorkFactory
    {
        public IUnitOfWork Create(IsolationLevel isolationLevel)
        {
            // Ignore isolation level for now. DbContext is NOT inherently thread-safe
            return new EntityFrameworkUnitOfWork(new MegaProjectContext());
        }

        public IUnitOfWork Create()
        {
            return Create(IsolationLevel.ReadCommitted);
        }
    }
}
