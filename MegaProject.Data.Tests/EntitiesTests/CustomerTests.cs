using System;
using MegaProject.Data.Entities;
using NUnit.Framework;

namespace MegaProject.Data.Tests.EntitiesTests
{
    [TestFixture]
    public class CustomerTests
    {
        [Test]
        public void CustomerShouldCreateIdentityWhenNotInitializedFromDatabase()
        {
            var customer = new Customer();
            Assert.AreNotEqual(Guid.Empty, customer.Id);
        }

        [Test]
        public void CustomerShouldRememberIdentityWhenInitializedFromDatabase()
        {
            var customer = new Customer();
            var guid = Guid.NewGuid();
            customer.Id = guid;

            Assert.AreEqual(guid, customer.Id);
        }

        [Test]
        public void CustomerShouldSetItselfAsAParentForItsOrders()
        {
            var customer = new Customer();
            var order = new Order();

            customer.AddOrder(order);
            Assert.AreSame(customer, order.Customer);
        }

        [Test]
        public void CustomerShouldAddOrderToOrders()
        {
            var customer = new Customer();
            var order = new Order();

            customer.AddOrder(order);
            CollectionAssert.Contains(customer.Orders, order);
        }
    }
}
