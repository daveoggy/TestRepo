using System;
using MegaProject.Data.Contracts;

namespace MegaProject.Data.EntityFramework
{
    public interface IEFUnitOfWork : IUnitOfWork, IDisposable
    {
        MegaProjectContext Context { get; }
    }
}
