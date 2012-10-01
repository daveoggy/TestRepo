
namespace MegaProject.Data.Entities
{
    public class OrderDetail
    {
        public virtual System.Guid OrderId { get; set; }
        public virtual decimal UnitPrice { get; set; }
        public virtual int Quantity { get; set; }
        public virtual decimal Discount { get; set; }

        public virtual Order Order { get; set; }

        // Ignored in EF, used in NHibernate
        public virtual int Int32OrderId { get; set; }
    }
}
