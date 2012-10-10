using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MegaProject.Data.Contracts;
using MegaProject.Data.Entities;
using MegaProject.Workflow.Activities;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;
using log4net;

namespace MegaProject.Activities.Tests
{
    public class SyncCompleteTests
    {
        [TestFixture]
        public class SyncCompleteShould
        {
            private IUnityContainer _container;
            private List<CustomerAudit> _changes;
            private Mock<IRepositoryBase> _mockRepo;
            private Mock<ILog> _mockLog;
            private Mock<Data.EntityFramework.Helpers.IRepositoryFactory> _efRepo;
            private Mock<Data.NHibernate.Helpers.IRepositoryFactory> _nhRepo;

            [SetUp]
            public void Setup()
            {
                _container = new UnityContainer();
                
                _mockLog = new Mock<ILog>();
                
                _changes = new List<CustomerAudit>();

                _mockRepo = new Mock<IRepositoryBase>();
                _mockRepo.Setup(r => r.All<CustomerAudit>()).Returns(_changes.AsQueryable());

                _efRepo = new Mock<Data.EntityFramework.Helpers.IRepositoryFactory>();
                _nhRepo = new Mock<Data.NHibernate.Helpers.IRepositoryFactory>();

                _efRepo.Setup(f => f.Create()).Returns(_mockRepo.Object);
                _nhRepo.Setup(f => f.Create()).Returns(_mockRepo.Object);

                _container.RegisterInstance(typeof(Data.EntityFramework.Helpers.IRepositoryFactory), _efRepo.Object);
                _container.RegisterInstance(typeof(Data.NHibernate.Helpers.IRepositoryFactory), _nhRepo.Object);
                _container.RegisterInstance(typeof(ILog), _mockLog.Object);
            }

            [Test]
            public void ReturnTheCorrectResultBasedOnPresenceOfNewChanges()
            {
                var stamp = new DateTime(2012, 10, 15, 15, 15, 15);
                var change = new CustomerAudit
                {
                    Email = "a@a.com",
                    IsSynced = false,
                    Type = "insert",
                    Added = stamp,
                };
                var change2 = new CustomerAudit
                {
                    Email = "a@a.com",
                    IsSynced = false,
                    Type = "insert",
                    Added = stamp.AddSeconds(20)
                };
                
                IDictionary<string, object> inputs = new Dictionary<string, object>
                {
                    {"Container", _container},
                    {"Change", change}
                };

                bool result = WorkflowInvoker.Invoke(new SyncComplete(), inputs);
                Assert.IsFalse(result);
                
                _changes.Add(change2);
                result = WorkflowInvoker.Invoke(new SyncComplete(), inputs);
                Assert.IsTrue(result);
            }

            [TestCase(ChangeSource.MSSQL)]
            [TestCase(ChangeSource.Oracle)]
            public void CreateTheCorrectRepositoryType(ChangeSource source)
            {
                var stamp = new DateTime(2012, 10, 15, 15, 15, 15);
                var change = new CustomerAudit
                {
                    Email = "a@a.com",
                    IsSynced = false,
                    Type = "insert",
                    Added = stamp,
                    Source = source
                };
                
                IDictionary<string, object> inputs = new Dictionary<string, object>
                {
                    {"Container", _container},
                    {"Change", change}
                };

                var result = WorkflowInvoker.Invoke(new SyncComplete(), inputs);
                if(source == ChangeSource.MSSQL)
                    _nhRepo.Verify(r => r.Create());
                else
                    _efRepo.Verify(r => r.Create());
            }
        }
    }
}
