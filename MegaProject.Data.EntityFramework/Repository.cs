using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using MegaProject.Data.Contracts;

namespace MegaProject.Data.EntityFramework
{
    public class Repository : IRepositoryBase
    {
        private readonly MegaProjectContext _context;
        public Repository(MegaProjectContext context)
        {
            if(context == null)
                throw new ArgumentNullException("context");
            
            _context = context;
        }

        public Repository(IEFUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException("unitOfWork");
            
            _context = unitOfWork.Context;
        }

        public Repository()
        {
            _context = new MegaProjectContext();
        }
        
        public IQueryable<TEntity> All<TEntity>()
            where TEntity : class
        {
            return _context.Set<TEntity>();
        }

        public IQueryable<TEntity> AllIncluding<TEntity>(params System.Linq.Expressions.Expression<Func<TEntity, object>>[] includeProperties)
             where TEntity : class
        {
            var query = _context.Set<TEntity>().AsQueryable();
            foreach (var prop in includeProperties)
            {
                query = query.Include(prop);
            }
            return query;
        }

        public TEntity GetById<TEntity>(int id)
             where TEntity : class
        {
            throw new NotImplementedException();
        }

        public TEntity GetById<TEntity>(Guid id)
             where TEntity : class
        {
            return _context.Set<TEntity>().Find(id);
        }

        public void Add<TEntity>(TEntity entity)
             where TEntity : class
        {
            var dbEntry = _context.Entry(entity);
            if(dbEntry.State != EntityState.Detached)
            {
                dbEntry.State = EntityState.Added;
            }
            else
            {
                _context.Set<TEntity>().Add(entity);
            }
        }

        public void Add<TEntity>(IEnumerable<TEntity> entities)
             where TEntity : class
        {
            foreach (var entity in entities)
                Add(entity);
        }

        public void Update<TEntity>(TEntity entity)
             where TEntity : class
        {
            var dbEntry = _context.Entry(entity);
            if(dbEntry.State == EntityState.Detached)
            {
                _context.Set<TEntity>().Attach(entity);
            }
            dbEntry.State = EntityState.Modified;
        }

        public void Delete<TEntity>(TEntity entity)
             where TEntity : class
        {
            var dbEntry = _context.Entry(entity);
            if(dbEntry.State != EntityState.Deleted)
            {
                dbEntry.State = EntityState.Deleted;
            }
            else
            {
                _context.Set<TEntity>().Attach(entity);
                _context.Set<TEntity>().Remove(entity);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            if(_context !=null)
                _context.Dispose();
        }
    }
}
