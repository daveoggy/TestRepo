using FluentNHibernate.Mapping;
using MegaProject.Data.Entities;

namespace MegaProject.Data.NHibernate.Configuration
{
    public class CustomerAuditMap : ClassMap<CustomerAudit>
    {
        public CustomerAuditMap()
        {
            Table("CUSTOMERS_AUDIT");
            Id(x => x.Id).Column("ID").GeneratedBy.Sequence("CUSTOMERS_AUDIT_SEQ");
            Map(x => x.Int32Id).Column("CUSTOMERID").Not.Nullable();
            Map(x => x.Email).Column("EMAIL").Not.Nullable();
            Map(x => x.CompanyName).Column("COMPANYNAME");
            Map(x => x.ContactName).Column("CONTACTNAME").Not.Nullable();
            Map(x => x.ContactTitle).Column("CONTACTTITLE");
            Map(x => x.Address).Column("ADDRESS");
            Map(x => x.City).Column("CITY");
            Map(x => x.Region).Column("REGION");
            Map(x => x.PostalCode).Column("POSTALCODE");
            Map(x => x.Country).Column("COUNTRY");
            Map(x => x.Phone).Column("PHONE");
            Map(x => x.Fax).Column("FAX");
            Map(x => x.Bool).Column("BOOL");

            Map(x => x.Added).Column("ADDED").Not.Nullable();
            Map(x => x.Type).Column("TTYPE").Not.Nullable();
            Map(x => x.IsSynced).Column("ISSYNCED").Not.Nullable();
            
        }
    }
}
