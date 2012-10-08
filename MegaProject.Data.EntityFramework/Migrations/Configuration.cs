using System;
using System.Data.Entity.Migrations;
using MegaProject.Data.Entities;

namespace MegaProject.Data.EntityFramework.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<MegaProjectContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(MegaProjectContext context)
        {

            context.Customers.AddOrUpdate(c => c.Id,
                                          new Customer()
                                              {
                                                  Id = Guid.Parse("04d8e001-0ea8-4880-9b0c-d1c4b9e7045d"),
                                                  ContactName = "Joe",
                                                  City = "Kiev",
                                                  Email = "joe@baba.com",
                                                  Phone = "9236499234"
                                              },
                                          new Customer()
                                              {
                                                  Id = Guid.Parse("24fdb27b-5850-424e-b58d-1a5b772b0640"),
                                                  ContactName = "Cindy Crawford",
                                                  City = "London",
                                                  Email = "yada@yadayadayada.com",
                                                  Phone = "928312934",
                                                  Bool = true,
                                                  CompanyName = "Company Ltd."
                                              },
                                          new Customer()
                                              {
                                                  Id = Guid.Parse("3285f9fd-34a5-48b0-8206-22ce06374472"),
                                                  ContactName = "Petya",
                                                  City = "New Delhi",
                                                  Email = "carramba@gl.net",
                                                  Fax = "8982376495"
                                              },
                                          new Customer()
                                              {
                                                  Id = Guid.Parse("64b58169-f4e1-4edc-9f0e-e204003fc28f"),
                                                  ContactName = "Vasiliy Zaycev",
                                                  City = "Stalingrad",
                                                  Email = "vz@nemcev.net",
                                                  Bool = false
                                              },
                                          new Customer()
                                              {
                                                  Id = Guid.Parse("18787398-4973-4be6-b811-878d68a44823"),
                                                  ContactName = "Another Joe",
                                                  City = "Singapore",
                                                  Email = "ajaj@gjgj.com",
                                                  Phone = "42342234",
                                                  ContactTitle = "Demon"
                                              });
            context.Orders.AddOrUpdate(o => o.Id,
                                        new Order()
                                        {
                                            Id = Guid.Parse("ca82d15b-105b-480b-8bc6-35e13e6e19d8"),
                                            CustomerId = Guid.Parse("04d8e001-0ea8-4880-9b0c-d1c4b9e7045d"),
                                            ShipName = "tratata",
                                            ShipAddress = "wwerwe rewr wer",
                                            ShipCity = "sdfsdf",
                                            ShipRegion = "erher",
                                            ShipPostalCode = "453453",
                                            ShipCountry = "sgaegar"
                                        },
                                        new Order()
                                        {
                                            Id = Guid.Parse("b7455505-b7c3-4569-a8b4-9dee3c619c2c"),
                                            CustomerId = Guid.Parse("04d8e001-0ea8-4880-9b0c-d1c4b9e7045d"),
                                            ShipName = "dfgjhr",
                                            ShipAddress = "drhgerh reher her",
                                            ShipCity = "erger",
                                            ShipRegion = "rjrtje",
                                            ShipPostalCode = "5645762",
                                            ShipCountry = "ghsfjtser"
                                        },
                                        new Order()
                                        {
                                            Id = Guid.Parse("ecfbd2fd-4030-48b3-82f0-7989efeadfd1"),
                                            CustomerId = Guid.Parse("3285f9fd-34a5-48b0-8206-22ce06374472"),
                                            ShipName = "fgjdfgjdfg",
                                            ShipAddress = "mdghdfy 4564 fgjhfgj",
                                            ShipCity = "yjtyj",
                                            ShipRegion = "jhrtjrtj",
                                            ShipPostalCode = "363634",
                                            ShipCountry = "tkethjdgfh"
                                        },
                                        new Order()
                                        {
                                            Id = Guid.Parse("43f4cbe1-0a62-4ebc-b40f-fdbd478771cf"),
                                            CustomerId = Guid.Parse("64b58169-f4e1-4edc-9f0e-e204003fc28f"),
                                            ShipName = "tratata",
                                            ShipAddress = "456 fdhsdhaer aerh",
                                            ShipCity = "dfhd",
                                            ShipRegion = "wajsf",
                                            ShipPostalCode = "56835",
                                            ShipCountry = "dfhadethergh"
                                        },
                                        new Order()
                                        {
                                            Id = Guid.Parse("b3afe514-8baa-422b-bd64-360eb13bc5a5"),
                                            CustomerId = Guid.Parse("64b58169-f4e1-4edc-9f0e-e204003fc28f"),
                                            ShipName = "hhklfe",
                                            ShipAddress = "fdhdfh 45452 dfhadfher",
                                            ShipCity = "dfjlyuhet",
                                            ShipRegion = "sefsefr",
                                            ShipPostalCode = "873134",
                                            ShipCountry = "tydqaqe"
                                        },
                                        new Order()
                                        {
                                            Id = Guid.Parse("74fa21a2-9710-41cc-890e-a48f0a1d6bd4"),
                                            CustomerId = Guid.Parse("64b58169-f4e1-4edc-9f0e-e204003fc28f"),
                                            ShipName = "tratata",
                                            ShipAddress = "twerty eyer 65",
                                            ShipCity = "eryr tj",
                                            ShipRegion = "rtjsfrgjhas",
                                            ShipPostalCode = "0875986",
                                            ShipCountry = "tydgyjsf"
                                        },
                                        new Order()
                                        {
                                            Id = Guid.Parse("7efa26f4-48e1-465f-8685-3e273e462d67"),
                                            CustomerId = Guid.Parse("24fdb27b-5850-424e-b58d-1a5b772b0640"),
                                            ShipName = "frjsrt",
                                            ShipAddress = "eagdfg eh aerhg 23",
                                            ShipCity = "fghjsf",
                                            ShipRegion = "frtjgtawe",
                                            ShipPostalCode = "467967",
                                            ShipCountry = "djsrtykswrt"
                                        });
            context.OrderDetails.AddOrUpdate(od => od.OrderId,
                                        new OrderDetail()
                                        {
                                            OrderId = Guid.Parse("ca82d15b-105b-480b-8bc6-35e13e6e19d8"),
                                            UnitPrice = 35.0m,
                                            Quantity = 345,
                                            Discount = 0.0m
                                        },
                                        new OrderDetail()
                                        {
                                            OrderId = Guid.Parse("b7455505-b7c3-4569-a8b4-9dee3c619c2c"),
                                            UnitPrice = 148.34m,
                                            Quantity = 21,
                                            Discount = 5.0m
                                        },
                                        new OrderDetail()
                                        {
                                            OrderId = Guid.Parse("43f4cbe1-0a62-4ebc-b40f-fdbd478771cf"),
                                            UnitPrice = 1649.0m,
                                            Quantity = 35,
                                            Discount = 9.7m
                                        },
                                        new OrderDetail()
                                        {
                                            OrderId = Guid.Parse("7efa26f4-48e1-465f-8685-3e273e462d67"),
                                            UnitPrice = 790.0m,
                                            Quantity = 15,
                                            Discount = 4.66m
                                        });
        }
    }
}
