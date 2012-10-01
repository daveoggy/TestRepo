using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using MegaProject.Data.Entities;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace MegaProject.Data.NHibernate.Helpers
{
    public class SeedingDatabaseInitializer
    {
        public void Initialize()
        {
            IPersistenceConfigurer cfg = OracleClientConfiguration
                .Oracle10
                .ConnectionString(
                    "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=127.0.0.1)(PORT=1521))(CONNECT_DATA=(SERVER=dedicated)(SERVICE_NAME=XE)));User ID=mproject;Password=vjqgfhjkm;")
                .ShowSql();
            
            var factory = Fluently.Configure(new global::NHibernate.Cfg.Configuration().Configure())
                            .Database(cfg)
                            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<NHibernateHelper>())
                            .ExposeConfiguration(BuildSchema)
                            .BuildSessionFactory();

            using(var session = factory.OpenSession())
            {
                Seed(session);
            }
        }

        private void BuildSchema(global::NHibernate.Cfg.Configuration config)
        {
            new SchemaExport(config).Create(false, true);
        }

        private void Seed(ISession session)
        {
            var customers = new[] {new Customer()
                                          {
                                              ContactName = "Joe",
                                              City = "Kiev",
                                              Email = "joe@baba.com",
                                              Phone = "9236499234"
                                          },
                                          new Customer()
                                          {
                                              ContactName = "Cindy Crawford",
                                              City = "London",
                                              Email = "yada@yadayadayada.com",
                                              Phone = "928312934",
                                              Bool = true,
                                              CompanyName = "Company Ltd."
                                          },
                                          new Customer()
                                          {
                                              ContactName = "Petya",
                                              City = "New Delhi",
                                              Email = "carramba@gl.net",
                                              Fax = "8982376495"
                                          },
                                          new Customer()
                                          {
                                              ContactName = "Vasiliy Zaycev",
                                              City = "Stalingrad",
                                              Email = "vz@nemcev.net",
                                              Bool = false
                                          },
                                          new Customer()
                                          {
                                              ContactName = "Another Joe",
                                              City = "Singapore",
                                              Email = "ajaj@gjgj.com",
                                              Phone = "42342234",
                                              ContactTitle = "Demon"
                                          }};
            var order1 = new Order()
                             {
                                 ShipName = "tratata",
                                 ShipAddress = "wwerwe rewr wer",
                                 ShipCity = "sdfsdf",
                                 ShipRegion = "erher",
                                 ShipPostalCode = "453453",
                                 ShipCountry = "sgaegar"
                             };
            order1.AddOrderDetail(new OrderDetail()
                                        {
                                            UnitPrice = 35.0m,
                                            Quantity = 345,
                                            Discount = 0.0m
                                        });
            customers[0].AddOrder(order1);
            order1 = new Order()
                         {
                             ShipName = "dfgjhr",
                             ShipAddress = "drhgerh reher her",
                             ShipCity = "erger",
                             ShipRegion = "rjrtje",
                             ShipPostalCode = "5645762",
                             ShipCountry = "ghsfjtser"
                         };
            order1.AddOrderDetail(new OrderDetail()
                                        {
                                            UnitPrice = 148.34m,
                                            Quantity = 21,
                                            Discount = 5.0m
                                        });
            customers[0].AddOrder(order1);
            customers[2].AddOrder(new Order()
                                        {
                                            ShipName = "fgjdfgjdfg",
                                            ShipAddress = "mdghdfy 4564 fgjhfgj",
                                            ShipCity = "yjtyj",
                                            ShipRegion = "jhrtjrtj",
                                            ShipPostalCode = "363634",
                                            ShipCountry = "tkethjdgfh"
                                        });
            order1 = new Order()
                         {
                             ShipName = "tratata",
                             ShipAddress = "456 fdhsdhaer aerh",
                             ShipCity = "dfhd",
                             ShipRegion = "wajsf",
                             ShipPostalCode = "56835",
                             ShipCountry = "dfhadethergh"
                         };
            order1.AddOrderDetail(new OrderDetail()
                                        {
                                            UnitPrice = 1649.0m,
                                            Quantity = 35,
                                            Discount = 9.7m
                                        });
            customers[3].AddOrder(order1);
            customers[3].AddOrder(new Order()
                                        {
                                            ShipName = "hhklfe",
                                            ShipAddress = "fdhdfh 45452 dfhadfher",
                                            ShipCity = "dfjlyuhet",
                                            ShipRegion = "sefsefr",
                                            ShipPostalCode = "873134",
                                            ShipCountry = "tydqaqe"
                                        });
            customers[3].AddOrder(new Order()
                                        {
                                            ShipName = "tratata",
                                            ShipAddress = "twerty eyer 65",
                                            ShipCity = "eryr tj",
                                            ShipRegion = "rtjsfrgjhas",
                                            ShipPostalCode = "0875986",
                                            ShipCountry = "tydgyjsf"
                                        });
            order1 = new Order()
                         {
                             ShipName = "frjsrt",
                             ShipAddress = "eagdfg eh aerhg 23",
                             ShipCity = "fghjsf",
                             ShipRegion = "frtjgtawe",
                             ShipPostalCode = "467967",
                             ShipCountry = "djsrtykswrt"
                         };
            order1.AddOrderDetail(new OrderDetail()
                                        {
                                            UnitPrice = 790.0m,
                                            Quantity = 15,
                                            Discount = 4.66m
                                        });
            
            using(var tx = session.BeginTransaction())
            {
                foreach (var c in customers)
                    session.Save(c);
                tx.Commit();
            }
        }
    }
}
