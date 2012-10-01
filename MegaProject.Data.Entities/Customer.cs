using System;
using System.Collections.Generic;

namespace MegaProject.Data.Entities
{
    public class Customer
    {
        private Guid _id;

        public Customer()
        {
            
        }

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

        public virtual string CompanyName { get; set; }
        public virtual string ContactName { get; set; }
        public virtual string ContactTitle { get; set; }
        public virtual string Address { get; set; }
        public virtual string City { get; set; }
        public virtual string Region { get; set; }
        public virtual string PostalCode { get; set; }
        public virtual string Country { get; set; }
        public virtual string Phone { get; set; }
        public virtual string Fax { get; set; }
        public virtual bool? Bool { get; set; }
        public virtual string Email { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        // Ignored in EF, used in NHibernate
        public virtual int Int32Id { get; set; }

        // Set up both sides of the relationship
        public virtual void AddOrder(Order order)
        {
            if (Orders == null)
                Orders = new List<Order>();
            order.Customer = this;
            Orders.Add(order);
        }
    }
}
