using System;
using System.Data;
using NHibernate;

namespace MegaProject.Data.NHibernate
{
    public class NHibernateUnitOfWork : INHUnitOfWork
    {
        private readonly ISession _session;
        private ITransaction _transaction;

        public NHibernateUnitOfWork(ISession session) : this(session, IsolationLevel.ReadCommitted)
        {     
        }
        
        public NHibernateUnitOfWork(ISession session, IsolationLevel isolationLevel)
        {
            if(session == null)
                throw new ArgumentNullException("Session");

            //CurrentSessionContext.Bind(session);
            _session = session;
            _transaction = _session.BeginTransaction(isolationLevel);
        }

        public ISession Session { get { return _session; } }
        
        public void Commit()
        {
            _transaction.Commit();
        }

        public void Dispose()
        {
            if(!_transaction.WasCommitted && !_transaction.WasRolledBack)
                _transaction.Rollback();

            _transaction.Dispose();
            _transaction = null;

            //CurrentSessionContext.Unbind(_session.SessionFactory);
            _session.Dispose();
        }
    }
}
