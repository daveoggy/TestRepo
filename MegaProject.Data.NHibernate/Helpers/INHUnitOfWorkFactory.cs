using System.Data;

namespace MegaProject.Data.NHibernate
{
    public interface INHUnitOfWorkFactory
    {
        INHUnitOfWork Create(IsolationLevel isolationLevel);
        INHUnitOfWork Create();
    }
}
