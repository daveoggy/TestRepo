using System;

namespace MegaProject.Data.Entities
{
    public class Order
    {
        private Guid _id;

        // Ignored in NHibernate, used in EF
        public virtual System.Guid CustomerId { get; set; }
        public virtual System.Guid Id
        {
            get
            {
                if (_id == Guid.Empty)
                    _id = Guid.NewGuid();

                return _id;
            }
            set { _id = value; }
        }

        public virtual string EmployeeId { get; set; }
        public virtual DateTime? OrderDate { get; set; }
        public virtual DateTime? RequiredDate { get; set; }
        public virtual DateTime? ShippedDate { get; set; }
        public virtual string ShipVia { get; set; }
        public virtual string Freight { get; set; }
        public virtual string ShipName { get; set; }
        public virtual string ShipAddress { get; set; }
        public virtual string ShipCity { get; set; }
        public virtual string ShipRegion { get; set; }
        public virtual string ShipPostalCode { get; set; }
        public virtual string ShipCountry { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual OrderDetail OrderDetail { get; set; }

        // Ignored in EF, used in NHibernate
        public virtual int Int32Id { get; set; }
        // Handle conversion from string database field to date
        public virtual string RequiredDateString
        {
            get { return (RequiredDate.HasValue) ? RequiredDate.Value.ToString("dd-MM-yyyy") : null; }
            set { RequiredDate = (value == null) ? (DateTime?)null : DateTime.ParseExact(value, "dd-MM-yyyy", null); }
        }

        public virtual void AddOrderDetail(OrderDetail detail)
        {
            detail.Order = this;
            this.OrderDetail = detail;
        }
    }
}
