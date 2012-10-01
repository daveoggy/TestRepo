using FluentNHibernate.Mapping;
using MegaProject.Data.Entities;

namespace MegaProject.Data.NHibernate.Configuration
{
    public class OrderDetailMap : ClassMap<OrderDetail>
    {
        public OrderDetailMap()
        {
            Table("ORDERDETAILS");
            Id(x => x.Int32OrderId).Column("ORDERID");//.GeneratedBy.Foreign("ORDERID");
			Map(x => x.UnitPrice).Column("UNITPRICE").Not.Nullable();
			Map(x => x.Quantity).Column("QUANTITY").Not.Nullable();
			Map(x => x.Discount).Column("DISCOUNT").Not.Nullable();

            References(x => x.Order).Column("ORDERID");
        }
    }
}
