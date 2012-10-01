using System;
using System.Data.Entity;
using MegaProject.Data.Entities;

namespace MegaProject.Data.EntityFramework
{
    public interface IMegaProjectContext : IDisposable
    {
        IDbSet<Customer> Customers { get; }
        IDbSet<Order> Orders { get; }
        IDbSet<OrderDetail> OrderDetails { get; }
        int SaveChanges();
        void SetModified(object entity);
        void SetAdd(object entity);
    }
}