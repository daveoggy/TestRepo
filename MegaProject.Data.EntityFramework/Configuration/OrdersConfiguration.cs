using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MegaProject.Data.Entities;

namespace MegaProject.Data.EntityFramework.Configuration
{
    internal class OrdersConfiguration : EntityTypeConfiguration<Order>
    {
        public OrdersConfiguration()
        {
            HasKey(o => o.Id).Property(o => o.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None)
                .HasColumnName("OrderID");
            Property(o => o.EmployeeId).HasMaxLength(50).HasColumnName("EmployeeID");
            Property(o => o.CustomerId).HasColumnName("CustomerID");
            Property(o => o.ShipName).HasMaxLength(50).IsRequired();
            Property(o => o.ShipAddress).HasMaxLength(200).IsRequired();
            Property(o => o.ShipCity).HasMaxLength(50).IsRequired();
            Property(o => o.ShipRegion).HasMaxLength(50).IsRequired();
            Property(o => o.ShipPostalCode).HasMaxLength(10).IsRequired();
            Property(o => o.ShipCountry).HasMaxLength(50).IsRequired();
            Property(o => o.ShipVia).HasMaxLength(50);
            Property(o => o.Freight).HasMaxLength(150);

            HasOptional(o => o.OrderDetail).WithRequired(od => od.Order).WillCascadeOnDelete(true);
            HasRequired(o => o.Customer).WithMany(c => c.Orders);

            // ignore NH id field
            Ignore(o => o.Int32Id);
            // ignore technical field for converting oracle string to datetime
            Ignore(o => o.RequiredDateString);
        }
    }
}
