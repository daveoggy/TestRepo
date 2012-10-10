using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MegaProject.Data.Entities;
using NH = MegaProject.Data.NHibernate;
using MegaProject.Workflow.Activities;
using EF = MegaProject.Data.EntityFramework;
using NUnit.Framework;

namespace MegaProject.Workflow.IntegrationTests
{
    [TestFixture]
    public class MainWorkflowTests
    {
        [Test]
        public void SynchronizingUpdatedCustomer()
        {
            // Get an existing customer from MSSQL and change it
            Customer cust;
            using(var repo = new EF.Repository())
            {
                cust = repo.All<Customer>().Single(c => c.Email == "joe@baba.com");
                cust.City = "Ololo";
                repo.Update(cust);
                repo.Save();
            }

            // Run the workflow
            WorkflowInvoker.Invoke(new Main());

            // Get same customer from Oracle and compare
            Customer cust2;
            using (var repo = new NH.Repository())
            {
                cust2 = repo.All<Customer>().Single(c => c.Email == "joe@baba.com");
            }

            Assert.AreEqual(cust.City, cust2.City);
        }

        [Test]
        public void SynchronizingUpdatedCustomerWithConflict()
        {
            // Get an existing customer from MSSQL and change it
            using(var repo = new EF.Repository())
            {
                Customer cust = repo.All<Customer>().Single(c => c.Email == "joe@baba.com");
                cust.City = "Bubu";
                repo.Update(cust);
                repo.Save();
            }
            // Give it some time
            Thread.Sleep(1000);
            // Get same customer from Oracle and introduce a conflicting change
            Customer cust2;
            using (var repo = new NH.Repository())
            {
                cust2 = repo.All<Customer>().Single(c => c.Email == "joe@baba.com");
                cust2.City = "Koko";
                repo.Update(cust2);
                repo.Save();
            }

            // Run the workflow
            WorkflowInvoker.Invoke(new Main());

            // Get the customer from MSSQL and compare
            Customer cust3;
            using (var repo = new EF.Repository())
            {
                cust3 = repo.All<Customer>().Single(c => c.Email == "joe@baba.com");
            }

            Assert.AreEqual(cust2.City, cust3.City);
        }

        [Test]
        public void SynchronizingInsertedCustomer()
        {
            // Generate email
            string email = string.Format("customer{0}@zizi.com", new Random().Next(100000, 999999));
            // Create a new Customer
            Customer x = new Customer {Email = email, ContactName = "New Customer"};

            // Insert it into Oracle (for a change :) )
            using (var repo = new NH.Repository())
            {
                repo.Add(x);
                repo.Save();
            }

            // Give it some time
            Thread.Sleep(1000);

            // Run the workflow
            WorkflowInvoker.Invoke(new Main());
            
            // Try to get it from MSSQL now
            Customer y;
            using (var repo = new EF.Repository())
            {
                y = repo.All<Customer>().SingleOrDefault(c => c.Email == email);
            }

            Assert.IsNotNull(y);
        }
    }
}
