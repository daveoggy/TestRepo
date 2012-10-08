using System;
using System.Activities;
using System.Collections.Generic;
using MegaProject.Data.Entities;
using MegaProject.Utilities.Unity;
using MegaProject.Workflow.Activities;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;
using log4net;

namespace MegaProject.Activities.Tests
{
    public class MergeChangesTests
    {
        [TestFixture]
        public class MergeChangesShould
        {
            private IUnityContainer _container;

            [TestFixtureSetUp]
            public void Configure()
            {
                _container = new UnityContainer();
                var logger = new Mock<ILog>();
                _container.RegisterInstance(typeof (ILog), logger.Object);
            }

            [Test]
            public void CorrectlyMergeIntersectingChanges()
            {
                var stamp = new DateTime(2012, 10, 15, 15, 15, 15);
                var ca1 = new CustomerAudit
                {
                    Email = "x1@x1.com",
                    ContactName = "x1",
                    Type = "insert",
                    Added = stamp
                };
                var ca21 = new CustomerAudit
                {
                    Email = "x2@x2.com",
                    ContactName = "x2",
                    Type = "insert",
                    Added = stamp.AddSeconds(5),
                    Source = ChangeSource.MSSQL
                };
                var ca22 = new CustomerAudit
                {
                    Email = "x2@x2.com",
                    ContactName = "x2",
                    Type = "insert",
                    Added = stamp.AddSeconds(10),
                    Source = ChangeSource.Oracle
                };
                var ca23 = new CustomerAudit
                {
                    Email = "x2@x2.com",
                    ContactName = "x2",
                    Type = "update",
                    Added = stamp.AddSeconds(20)
                };
                var ca31 = new CustomerAudit
                {
                    Email = "x3@x3.com",
                    ContactName = "x3",
                    Type = "update",
                    Added = stamp.AddSeconds(15)
                };
                var ca32 = new CustomerAudit
                {
                    Email = "x3@x3.com",
                    ContactName = "x3",
                    Type = "update",
                    Added = stamp.AddSeconds(30)
                };
                var ca4 = new CustomerAudit
                {
                    Email = "x4@x4.com",
                    ContactName = "x4",
                    Type = "insert",
                    Added = stamp.AddSeconds(25)
                };
                var ca5 = new CustomerAudit
                {
                    Email = "x5@x5.com",
                    ContactName = "x5",
                    Type = "update",
                    Added = stamp.AddSeconds(35)
                };

                var first = new List<CustomerAudit>
                {
                    ca1, ca22, ca4, ca32
                };
                var second = new List<CustomerAudit>
                {
                    ca21, ca31, ca23, ca5
                };

                IDictionary<string, object> inputs = new Dictionary<string, object>
                {
                    {"Container", _container},
                    {"OracleChanges", first},
                    {"MSSQLChanges", second}
                };

                var outputs = WorkflowInvoker.Invoke(new MergeChanges(), inputs);
                var expected = new List<CustomerAudit> { ca1, ca23, ca4, ca32, ca5 };

                CollectionAssert.AreEqual(expected, outputs);
            }

            [Test]
            public void MergeListsIfTheyDoNotIntersect()
            {
                var stamp = new DateTime(2012, 10, 15, 15, 15, 15);
                var ca1 = new CustomerAudit
                {
                    Email = "x1@x2.com",
                    ContactName = "xxx",
                    Added = stamp
                };
                var ca2 = new CustomerAudit
                {
                    Email = "x2@x4.com",
                    ContactName = "yyy",
                    Added = stamp.AddSeconds(20)
                };
                var ca3 = new CustomerAudit
                {
                    Email = "x5@x9.com",
                    ContactName = "zzz",
                    Added = stamp.AddSeconds(-43)
                };
                var first = new List<CustomerAudit>
                {
                    ca2
                };
                var second = new List<CustomerAudit>
                {
                    ca3,
                    ca1
                };
                IDictionary<string, object> inputs = new Dictionary<string, object>
                {
                    {"Container", _container},
                    {"OracleChanges", first},
                    {"MSSQLChanges", second}
                };

                var outputs = WorkflowInvoker.Invoke(new MergeChanges(), inputs);
                var expected = new List<CustomerAudit> { ca3, ca1, ca2};
                CollectionAssert.AreEqual(expected, outputs);
            }

            [Test]
            public void IgnoreEmptyChangesLists()
            {
                var stamp = new DateTime(2012, 10, 15, 15, 15, 15);
                var ca1 = new CustomerAudit
                {
                    Email = "x1@x2.com",
                    ContactName = "xxx",
                    Added = stamp
                };
                var ca2 = new CustomerAudit
                {
                    Email = "x2@x4.com",
                    ContactName = "yyy",
                    Added = stamp.AddSeconds(20)
                };
                var empty = new List<CustomerAudit>();
                var other = new List<CustomerAudit>
                {
                    ca2,
                    ca1
                };
                IDictionary<string, object> inputs = new Dictionary<string, object>
                {
                    {"Container", _container},
                    {"OracleChanges", empty},
                    {"MSSQLChanges", other}
                };

                var outputs = WorkflowInvoker.Invoke(new MergeChanges(), inputs);
                Assert.AreSame(ca1, outputs[0]);
                Assert.AreSame(ca2, outputs[1]);
            }
        }
    }
}
