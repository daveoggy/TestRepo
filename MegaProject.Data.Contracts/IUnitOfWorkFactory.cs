using System.Data;

namespace MegaProject.Data.Contracts
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create(IsolationLevel isolationLevel);
        IUnitOfWork Create();
    }
}
