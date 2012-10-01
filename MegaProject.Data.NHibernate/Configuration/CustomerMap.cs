using FluentNHibernate.Mapping;
using MegaProject.Data.Entities;

namespace MegaProject.Data.NHibernate.Configuration
{
    public class CustomerMap : ClassMap<Customer>
    {
        public CustomerMap()
        {
            Table("CUSTOMERS");
            Id(x => x.Int32Id).Column("CUSTOMERID").GeneratedBy.Sequence("CUSTOMERS_SEQ");
            Map(x => x.Email).Column("EMAIL").Unique().Not.Nullable();
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

            HasMany(x => x.Orders).KeyColumn("CUSTOMERID")
                .Inverse()
                .Cascade.All();
        }
    }
}
