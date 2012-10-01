using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MegaProject.Data.NHibernate
{
    public interface INHUnitOfWorkFactory
    {
        INHUnitOfWork Create(IsolationLevel isolationLevel);
        INHUnitOfWork Create();
    }
}
