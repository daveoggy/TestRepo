using System.Data;

namespace MegaProject.Data.EntityFramework
{
    public interface IEFUnitOfWorkFactory
    {
        IEFUnitOfWork Create(IsolationLevel isolationLevel);
        IEFUnitOfWork Create();
    }
}
