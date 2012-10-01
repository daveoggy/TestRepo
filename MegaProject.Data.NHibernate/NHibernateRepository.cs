using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MegaProject.Data.Contracts;
using NHibernate;
using NHibernate.Linq;

namespace MegaProject.Data.NHibernate
{
    public class NHibernateRepository<TEntity> : IRepository<TEntity>, IDisposable
        where TEntity : class
    {
        private readonly ISession _session;

        public NHibernateRepository(NHibernateUnitOfWork unitOfWork)
        {
            if(unitOfWork == null)
                throw new ArgumentNullException("unitOfWork");

            //CurrentSessionContext.Bind(session);
            _session = unitOfWork.Session;
        }

        public IQueryable<TEntity> All
        {
            get { return _session.Query<TEntity>(); }
        }

        public IQueryable<TEntity> AllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = _session.Query<TEntity>();
            foreach (var prop in includeProperties)
            {
                query = query.Fetch(prop);
            }
            return query;
        }

        public TEntity GetById(int id)
        {
            return _session.Get<TEntity>(id);
        }

        public TEntity GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Add(TEntity entity)
        {
            _session.Save(entity);
        }

        public void Add(IEnumerable<TEntity> entities)
        {
            foreach(var entity in entities)
                this.Add(entity);
        }

        public void Update(TEntity entity)
        {
            _session.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _session.Delete(entity);
        }

        public void Dispose()
        {
            // We do not own the session
        }


        
    }
}
