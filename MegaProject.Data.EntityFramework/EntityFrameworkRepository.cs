using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using MegaProject.Data.Contracts;

namespace MegaProject.Data.EntityFramework
{
    public class EntityFrameworkRepository<TEntity> : IRepository<TEntity>, IDisposable
        where TEntity : class
    {
        public EntityFrameworkRepository(IEFUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException("UnitOfWork");

            Context = unitOfWork.Context;
            EntitySet = unitOfWork.Context.Set<TEntity>();
        }


        protected IMegaProjectContext Context { get; set; }
        protected IDbSet<TEntity> EntitySet { get; set; }

        public virtual IQueryable<TEntity> All
        {
            get { return EntitySet; }
        }

        public IQueryable<TEntity> AllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = EntitySet.AsQueryable();
            foreach(var prop in includeProperties)
            {
                query = query.Include(prop);
            }
            return query;
        }

        public virtual TEntity GetById(int id)
        {
            throw new NotImplementedException();
        }

        public virtual TEntity GetById(Guid id)
        {
            return EntitySet.Find(id);
        }

        public virtual void Add(TEntity entity)
        {
            //var dbEntry = Context.Entry(entity);
            //if(dbEntry.State != EntityState.Detached)
            //{
            //    dbEntry.State = EntityState.Added;
            //}
            //else
            //{
            //    EntitySet.Add(entity);
            //}
            Context.SetAdd(entity);
        }

        public void Add(IEnumerable<TEntity> entities)
        {
            foreach(var entity in entities)
                this.Add(entity);
        }

        public virtual void Update(TEntity entity)
        {
            //var dbEntry = Context.Entry(entity);
            //if(dbEntry.State == EntityState.Detached)
            //{
            //    EntitySet.Attach(entity);
            //}
            //dbEntry.State = EntityState.Modified;
            Context.SetModified(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            //var dbEntry = Context.Entry(entity);
            //if(dbEntry.State != EntityState.Deleted)
            //{
            //    dbEntry.State = EntityState.Deleted;
            //}
            //else
            //{
            //    EntitySet.Attach(entity);
            //    EntitySet.Remove(entity);
            //}
            EntitySet.Remove(entity);
        }

        #region IDisposable pattern
        private bool _disposed = false;
        

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        } 
        #endregion



        
    }
}
