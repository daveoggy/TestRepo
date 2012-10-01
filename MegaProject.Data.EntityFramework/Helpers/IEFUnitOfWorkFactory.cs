using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MegaProject.Data.Contracts;

namespace MegaProject.Data.EntityFramework
{
    public interface IEFUnitOfWorkFactory
    {
        IEFUnitOfWork Create(IsolationLevel isolationLevel);
        IEFUnitOfWork Create();
    }
}
