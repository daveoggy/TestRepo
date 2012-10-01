using System.Data.Entity.ModelConfiguration;
using MegaProject.Data.Entities;

namespace MegaProject.Data.EntityFramework.Configuration
{
    internal class OrderDetailsConfiguration : EntityTypeConfiguration<OrderDetail>
    {
        public OrderDetailsConfiguration()
        {
            HasKey(od => od.OrderId)
               .Property(od => od.OrderId).HasColumnName("OrderID");
            HasRequired(od => od.Order).WithOptional(o => o.OrderDetail);
            Property(od => od.UnitPrice).IsRequired();
            Property(od => od.Discount).IsRequired();
            Property(od => od.Quantity).IsRequired();

            Ignore(od => od.Int32OrderId);
        }
    }
}
