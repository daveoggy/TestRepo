using System.Data;

namespace MegaProject.Data.EntityFramework.Helpers
{
    public class MegaEntityFrameworkUnitOfWorkFactory : IEFUnitOfWorkFactory
    {
        public IEFUnitOfWork Create(IsolationLevel isolationLevel)
        {
            // Ignore isolation level for now. DbContext is NOT inherently thread-safe
            return new EntityFrameworkUnitOfWork(new MegaProjectContext());
        }

        public IEFUnitOfWork Create()
        {
            return Create(IsolationLevel.ReadCommitted);
        }
    }
}
