using System;
using MegaProject.Data.Contracts;
using NHibernate;

namespace MegaProject.Data.NHibernate
{
    public interface INHUnitOfWork : IUnitOfWork, IDisposable
    {
        ISession Session { get; }
    }
}
