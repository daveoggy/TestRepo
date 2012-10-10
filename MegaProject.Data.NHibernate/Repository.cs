using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MegaProject.Data.Contracts;
using MegaProject.Data.NHibernate.Helpers;
using NHibernate;
using NHibernate.Context;
using NHibernate.Linq;

namespace MegaProject.Data.NHibernate
{
    public class Repository : IRepositoryBase
    {
        private readonly ISession _session;
        private bool _shouldDispose = false;
        private ITransaction _transaction;

        public Repository(INHUnitOfWork unitOfWork)
        {
            if(unitOfWork == null)
                throw new ArgumentNullException("unitOfWork");

            //CurrentSessionContext.Bind(session);
            _session = unitOfWork.Session;
        }

        public Repository()
        {
            //try to grab current session if any
            ISession session;
            try
            {
                session = NHibernateHelper.Factory.GetCurrentSession();
                _session = session;
            }
            catch (HibernateException)
            {
                // There was no current session, create our own
                session = NHibernateHelper.Factory.OpenSession();
                //CurrentSessionContext.Bind(session);
                _session = session;
                
                _transaction = session.BeginTransaction();
                _shouldDispose = true;
            }

        }

        public Repository(ISession session)
        {
            if (session == null)
                throw new ArgumentNullException("session");

            //CurrentSessionContext.Bind(session);
            _session = session;
        }

        public IQueryable<TEntity> All<TEntity>()
            where TEntity : class
        {
            return _session.Query<TEntity>();
        }

        public IQueryable<TEntity> AllIncluding<TEntity>(params Expression<Func<TEntity, object>>[] includeProperties)
            where TEntity : class
        {
            var query = _session.Query<TEntity>();
            foreach (var prop in includeProperties)
            {
                query = query.Fetch(prop);
            }
            return query;
        }

        public TEntity GetById<TEntity>(int id)
            where TEntity : class
        {
            return _session.Get<TEntity>(id);
        }

        public TEntity GetById<TEntity>(Guid id)
            where TEntity : class
        {
            throw new NotImplementedException();
        }

        public void Add<TEntity>(TEntity entity)
            where TEntity : class
        {
            _session.Save(entity);
        }

        public void Add<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : class
        {
            foreach(var entity in entities)
                Add(entity);
        }

        public void Update<TEntity>(TEntity entity)
            where TEntity : class
        {
            _session.Update(entity);
        }

        public void Delete<TEntity>(TEntity entity)
            where TEntity : class
        {
            _session.Delete(entity);
        }

        public void Save()
        {
            if(_transaction != null)
                _transaction.Commit();
        }

        public void Dispose()
        {
            if(_shouldDispose)
            {
                if (_transaction != null && !_transaction.WasCommitted && !_transaction.WasRolledBack)
                    _transaction.Rollback();

                if (_transaction != null)
                    _transaction.Dispose();
                _transaction = null;

                //CurrentSessionContext.Unbind(_session.SessionFactory);
                _session.Dispose();
            }
        }


        
    }
}
