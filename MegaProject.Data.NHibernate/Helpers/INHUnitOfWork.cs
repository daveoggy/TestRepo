using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MegaProject.Data.Contracts;
using NHibernate;

namespace MegaProject.Data.NHibernate
{
    public interface INHUnitOfWork : IUnitOfWork, IDisposable
    {
        ISession Session { get; }
    }
}
