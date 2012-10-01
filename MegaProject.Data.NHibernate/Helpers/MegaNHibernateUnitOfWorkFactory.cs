using System.Data;
using MegaProject.Data.Contracts;

namespace MegaProject.Data.NHibernate.Helpers
{
    public class MegaNHibernateUnitOfWorkFactory : INHUnitOfWorkFactory
    {
        public INHUnitOfWork Create(IsolationLevel isolationLevel)
        {
            return new NHibernateUnitOfWork(NHibernateHelper.Factory.OpenSession(), isolationLevel);
        }

        public INHUnitOfWork Create()
        {
            return Create(IsolationLevel.ReadCommitted);
        }
    }
}
