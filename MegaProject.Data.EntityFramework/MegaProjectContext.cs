using System.Data;
using System.Data.Entity;
using MegaProject.Data.Entities;

namespace MegaProject.Data.EntityFramework
{
    public class MegaProjectContext : DbContext, IMegaProjectContext
    {
        public IDbSet<Customer> Customers { get; set; }
        public IDbSet<Order> Orders { get; set; }
        public IDbSet<OrderDetail> OrderDetails { get; set; }

        public MegaProjectContext()
            : base("MegaProjectContext")
        {
        }
        
        public MegaProjectContext(string connectionName) : base(connectionName)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new Configuration.CustomersConfiguration());
            modelBuilder.Configurations.Add(new Configuration.OrdersConfiguration());
            modelBuilder.Configurations.Add(new Configuration.OrderDetailsConfiguration());

            
        }


        public void SetModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

        public void SetAdd(object entity)
        {
            Entry(entity).State = EntityState.Added;
        }
    }
}
