using System;

namespace MegaProject.Data.Entities
{
    public class CustomerAudit
    {
        public virtual int Id { get; set; }
        public virtual System.Guid CustomerId { get; set; }
        
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
        
        public virtual DateTime Added { get; set; }
        public virtual string Type { get; set; }
        public virtual bool IsSynced { get; set; }

        // Ignored in EF, used in NHibernate
        public virtual int Int32Id { get; set; }

        // Used for technical purposes, not persisted
        public virtual ChangeSource Source { get; set; }
    }
}
