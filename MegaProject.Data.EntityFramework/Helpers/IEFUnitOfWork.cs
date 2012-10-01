using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MegaProject.Data.Contracts;

namespace MegaProject.Data.EntityFramework
{
    public interface IEFUnitOfWork : IUnitOfWork, IDisposable
    {
        MegaProjectContext Context { get; }
    }
}
