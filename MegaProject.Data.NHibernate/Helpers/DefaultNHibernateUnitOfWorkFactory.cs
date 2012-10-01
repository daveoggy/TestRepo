using System.Data;
using MegaProject.Data.Contracts;

namespace MegaProject.Data.NHibernate.Helpers
{
    public class DefaultNHibernateUnitOfWorkFactory : IUnitOfWorkFactory
    {
        public IUnitOfWork Create(IsolationLevel isolationLevel)
        {
            return new NHibernateUnitOfWork(NHibernateHelper.Factory.OpenSession(), isolationLevel);
        }

        public IUnitOfWork Create()
        {
            return Create(IsolationLevel.ReadCommitted);
        }
    }
}
