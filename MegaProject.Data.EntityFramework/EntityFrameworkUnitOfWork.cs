using System;

namespace MegaProject.Data.EntityFramework
{
    public class EntityFrameworkUnitOfWork : IEFUnitOfWork, IDisposable
    {
        public MegaProjectContext Context { get; private set; }
        //protected IRepositoryProvider RepositoryProvider { get; set; }

        public EntityFrameworkUnitOfWork()
        {
            Context = new MegaProjectContext();
        }
        
        public EntityFrameworkUnitOfWork(MegaProjectContext context)//, IRepositoryProvider repositoryProvider)
        {
            Context = context;

            //repositoryProvider.Context = Context;
            //RepositoryProvider = repositoryProvider;
        }

        //private IRepository<TEntity> GetStandardRepository<TEntity>() where TEntity : class
        //{
        //    return RepositoryProvider.GetRepositoryForEntityType<TEntity>();
        //}

        public void Commit()
        {
            Context.SaveChanges();
        }

        //public IRepository<Customer> Customers
        //{
        //    get { return GetStandardRepository<Customer>(); }
        //}

        //public IRepository<Order> Orders
        //{
        //    get { return GetStandardRepository<Order>(); }
        //}

        //public IRepository<OrderDetail> OrderDetails
        //{
        //    get { return GetStandardRepository<OrderDetail>(); }
        //}

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Context != null)
                {
                    Context.Dispose();
                }
            }
        }

        #endregion
    }
}
