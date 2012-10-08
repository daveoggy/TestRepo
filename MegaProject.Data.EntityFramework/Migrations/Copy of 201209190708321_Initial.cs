//using System.Data.Entity.Migrations;

//namespace MegaProject.Data.EntityFramework.Migrations
//{
//    public partial class Initial : DbMigration
//    {
//        public override void Up()
//        {
//            CreateTable(
//                "dbo.Customers",
//                c => new
//                    {
//                        CustomerID = c.Guid(nullable: false),
//                        CompanyName = c.String(maxLength: 150),
//                        ContactName = c.String(nullable: false, maxLength: 50),
//                        ContactTitle = c.String(maxLength: 50),
//                        Address = c.String(maxLength: 200),
//                        City = c.String(maxLength: 50),
//                        Region = c.String(maxLength: 50),
//                        PostalCode = c.String(maxLength: 10),
//                        Country = c.String(maxLength: 50),
//                        Phone = c.String(maxLength: 50),
//                        Fax = c.String(maxLength: 50),
//                        Bool = c.Boolean(),
//                        Email = c.String(nullable: false, maxLength: 100),
//                    })
//                .PrimaryKey(t => t.CustomerID)
//                .Index(c => c.Email);
            
//            CreateTable(
//                "dbo.Orders",
//                c => new
//                    {
//                        OrderID = c.Guid(nullable: false),
//                        CustomerID = c.Guid(nullable: false),
//                        EmployeeID = c.String(maxLength: 50),
//                        OrderDate = c.DateTime(),
//                        RequiredDate = c.DateTime(),
//                        ShippedDate = c.DateTime(),
//                        ShipVia = c.String(maxLength: 50),
//                        Freight = c.String(maxLength: 150),
//                        ShipName = c.String(nullable: false, maxLength: 50),
//                        ShipAddress = c.String(nullable: false, maxLength: 200),
//                        ShipCity = c.String(nullable: false, maxLength: 50),
//                        ShipRegion = c.String(nullable: false, maxLength: 50),
//                        ShipPostalCode = c.String(nullable: false, maxLength: 10),
//                        ShipCountry = c.String(nullable: false, maxLength: 50),
//                    })
//                .PrimaryKey(t => t.OrderID)
//                .ForeignKey("dbo.Customers", t => t.CustomerID, cascadeDelete: true)
//                .Index(t => t.CustomerID);
            
//            CreateTable(
//                "dbo.OrderDetails",
//                c => new
//                    {
//                        OrderID = c.Guid(nullable: false),
//                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
//                        Quantity = c.Int(nullable: false),
//                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
//                    })
//                .PrimaryKey(t => t.OrderID)
//                .ForeignKey("dbo.Orders", t => t.OrderID, cascadeDelete: true)
//                .Index(t => t.OrderID);
            
//        }
        
//        public override void Down()
//        {
//            DropIndex("dbo.OrderDetails", new[] { "OrderID" });
//            DropIndex("dbo.Orders", new[] { "CustomerID" });
//            DropForeignKey("dbo.OrderDetails", "OrderID", "dbo.Orders");
//            DropForeignKey("dbo.Orders", "CustomerID", "dbo.Customers");
//            DropTable("dbo.OrderDetails");
//            DropTable("dbo.Orders");
//            DropTable("dbo.Customers");
//        }
//    }
//}
