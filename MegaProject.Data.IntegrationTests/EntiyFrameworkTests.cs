using System.Data.Entity;
using System.Linq;
using MegaProject.Data.Entities;
using MegaProject.Data.EntityFramework;
using MegaProject.Data.EntityFramework.Configuration;
using MegaProject.Data.EntityFramework.Helpers;
using NUnit.Framework;

namespace MegaProject.Data.IntegrationTests
{
    [TestFixture]
    public class EntiyFrameworkTests
    {
        [SetUp]
        public void DbSetup()
        {
            Database.SetInitializer(new SeedingDatabaseInitializer());
            using (var ctx = new MegaProjectContext())
            {
                ctx.Database.Initialize(true);
            }
        }

        [Test]
        public void Can_Get_Customers_From_Database()
        {
            using (var repo = new RepositoryFactory().Create())
            {
                var customers = repo.All<Customer>().ToList();
                CollectionAssert.IsNotEmpty(customers);
            }
        }

        [Test]
        public void Can_Save_New_Customer()
        {
            var customer = new Customer
            {
                ContactName = "johnny",
                Email = "sc@sdfhlksj.com"
            };

            using (var repo = new RepositoryFactory().Create())
            {
                repo.Add(customer);
                repo.Save();
            }

            using (var repo = new RepositoryFactory().Create())
            {
                var cust = repo.All<Customer>().SingleOrDefault(c => c.Email == "sc@sdfhlksj.com");
                Assert.IsNotNull(cust);
            }
        }

        [Test]
        public void Can_Save_New_Customer_With_Orders_And_Details()
        {
            var customer = GetCustomer();

            using (var repo = new RepositoryFactory().Create())
            {
                repo.Add(customer);
                repo.Save();
            }

            using (var repo = new RepositoryFactory().Create())
            {
                var cust = repo.All<Customer>().SingleOrDefault(c => c.Email == "sc@sdfhlksj.com");
                Assert.IsNotNull(cust);
                CollectionAssert.IsNotEmpty(cust.Orders);
                Assert.IsNotNull(cust.Orders.First(o => o.OrderDetail != null).OrderDetail);
            }
        }

        [Test]
        public void Can_Use_UoW_To_Remove_And_Add_Customer_In_A_Transaction()
        {
            using (var uow = new MegaEntityFrameworkUnitOfWorkFactory().Create())
            {
                var repo = new RepositoryFactory().Create(uow);
                var c1 = repo.All<Customer>().Single(c => c.Email == "vz@nemcev.net");
                repo.Delete(c1);
                var c2 = GetCustomer();
                repo.Add(c2);
                uow.Commit();
            }

            using (var repo = new RepositoryFactory().Create())
            {
                var x = repo.All<Customer>().SingleOrDefault(c => c.Email == "vz@nemcev.net");
                Assert.IsNull(x);
                var y = repo.All<Customer>().Any(c => c.Country == "Italy");
                Assert.IsTrue(y);
            }
        }

        private Customer GetCustomer()
        {
            var customer = new Customer
            {
                ContactName = "johnny",
                Email = "sc@sdfhlksj.com",
                Country = "Italy"
            };

            var order1 = new Order
            {
                ShipName = "fjh rty",
                ShipAddress = "374 drgiaeroiu",
                ShipCity = "rtuket",
                ShipRegion = "reag qae",
                ShipPostalCode = "34573",
                ShipCountry = "tyjtwere"
            };

            var order2 = new Order
            {
                ShipName = "ghdgh",
                ShipAddress = "fjsr4 4545 fghsf",
                ShipCity = "fyjshrk",
                ShipRegion = "tdguyket",
                ShipPostalCode = "9345672",
                ShipCountry = "ryjsytr"
            };

            var detail = new OrderDetail
            {
                UnitPrice = 90.4m,
                Quantity = 28,
                Discount = 67.66m
            };

            order1.AddOrderDetail(detail);
            customer.AddOrder(order1);
            customer.AddOrder(order2);

            return customer;
        }
    }
}
