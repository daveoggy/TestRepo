using FluentNHibernate.Mapping;
using MegaProject.Data.Entities;

namespace MegaProject.Data.NHibernate.Configuration
{
    public class OrderMap : ClassMap<Order>
    {
        public OrderMap()
        {
            Table("ORDERS");
            Id(x => x.Int32Id).Column("ORDERID").GeneratedBy.Sequence("ORDERS_SEQ");
			Map(x => x.EmployeeId).Column("EMPLOYEEID");
			Map(x => x.OrderDate).Column("ORDERDATE");
			Map(x => x.RequiredDateString).Column("REQUIREDDATE");
			Map(x => x.ShippedDate).Column("SHIPPEDDATE");
			Map(x => x.ShipVia).Column("SHIPVIA");
			Map(x => x.Freight).Column("FREIGHT");
			Map(x => x.ShipName).Column("SHIPNAME").Not.Nullable();
			Map(x => x.ShipAddress).Column("SHIPADDRESS").Not.Nullable();
			Map(x => x.ShipCity).Column("SHIPCITY").Not.Nullable();
			Map(x => x.ShipRegion).Column("SHIPREGION").Not.Nullable();
			Map(x => x.ShipPostalCode).Column("SHIPPOSTALCODE").Not.Nullable();
			Map(x => x.ShipCountry).Column("SHIPCOUNTRY").Not.Nullable();

            References(x => x.Customer).Column("CUSTOMERID");

            HasOne(x => x.OrderDetail).Cascade.All();
        }
    }
}
