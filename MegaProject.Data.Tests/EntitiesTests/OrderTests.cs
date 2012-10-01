using System;
using MegaProject.Data.Entities;
using NUnit.Framework;

namespace MegaProject.Data.Tests.EntitiesTests
{
    [TestFixture]
    public class OrderTests
    {
        [Test]
        public void CustomerShouldCreateIdentityWhenNotInitializedFromDatabase()
        {
            var order = new Order();
            Assert.AreNotEqual(Guid.Empty, order.Id);
        }

        [Test]
        public void CustomerShouldRememberIdentityWhenInitializedFromDatabase()
        {
            var order = new Order();
            var guid = Guid.NewGuid();
            order.Id = guid;

            Assert.AreEqual(guid, order.Id);
        }

        [Test]
        public void OrderShouldAcceptRequiredDateAsNull()
        {
            var order = new Order();
            order.RequiredDate = null;
            Assert.IsNull(order.RequiredDateString);
        }

        [Test]
        public void OrderShouldAcceptRequiredDateAsStringAndSaveItAsADateTime()
        {
            var order = new Order();
            var date = new DateTime(2009, 9, 3);
            var stringDate = "03-09-2009";

            order.RequiredDateString = stringDate;
            Assert.AreEqual(date, order.RequiredDate);
        }

        [Test]
        public void OrderShouldAcceptRequiredDateAsDateTimeAndSaveItAsAString()
        {
            var order = new Order();
            var date = new DateTime(2009, 9, 3);
            var stringDate = "03-09-2009";

            order.RequiredDate = date;
            Assert.AreEqual(stringDate, order.RequiredDateString);
        }

        [Test]
        public void OrderShouldSetItselfAsParentForOrderDetail()
        {
            var order = new Order();
            var detail = new OrderDetail();

            order.AddOrderDetail(detail);
            Assert.AreSame(order, detail.Order);
        }

        [Test]
        public void OrderShouldAddOrderDetail()
        {
            var order = new Order();
            var detail = new OrderDetail();

            order.AddOrderDetail(detail);
            Assert.AreSame(detail, order.OrderDetail);
        }
    }
}