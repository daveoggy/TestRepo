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
    public class SyncChangeTests
    {
        [TestFixture]
        public class SyncChangeShould
        {
            private IUnityContainer _container;
            private Mock<IRepositoryBase> _mockRepo;
            private Mock<ILog> _mockLog;
            private Mock<Data.EntityFramework.Helpers.IRepositoryFactory> _efRepo;
            private Mock<Data.NHibernate.Helpers.IRepositoryFactory> _nhRepo;
            private List<Customer> _customers;
            private DateTime _stamp;
            private CustomerAudit _change;
            private IDictionary<string, object> _inputs;

            [SetUp]
            public void Setup()
            {
                _container = new UnityContainer();
                var stamp = new DateTime(2012, 10, 15, 15, 15, 15);
                _mockLog = new Mock<ILog>();
                _customers = new List<Customer>();
                
                _mockRepo = new Mock<IRepositoryBase>();
                _mockRepo.Setup(r => r.All<Customer>()).Returns(_customers.AsQueryable());

                _efRepo = new Mock<Data.EntityFramework.Helpers.IRepositoryFactory>();
                _nhRepo = new Mock<Data.NHibernate.Helpers.IRepositoryFactory>();

                _efRepo.Setup(f => f.Create()).Returns(_mockRepo.Object);
                _nhRepo.Setup(f => f.Create()).Returns(_mockRepo.Object);

                _container.RegisterInstance(typeof(Data.EntityFramework.Helpers.IRepositoryFactory), _efRepo.Object);
                _container.RegisterInstance(typeof(Data.NHibernate.Helpers.IRepositoryFactory), _nhRepo.Object);
                _container.RegisterInstance(typeof(ILog), _mockLog.Object);


                _stamp = new DateTime(2012, 10, 15, 15, 15, 15);
                _change = new CustomerAudit
                {
                    Email = "a@a.com",
                    IsSynced = false,
                    Type = "insert",
                    Added = stamp,
                    Source = ChangeSource.MSSQL
                };

                _inputs = new Dictionary<string, object>
                {
                    {"Container", _container},
                    {"Change", _change}
                };
            }

            [TestCase(ChangeSource.MSSQL)]
            [TestCase(ChangeSource.Oracle)]
            public void CreateTheCorrectRepositoryType(ChangeSource source)
            {
                _change.Source = source;
                WorkflowInvoker.Invoke(new SyncChange(), _inputs);
                if (source == ChangeSource.MSSQL)
                    _nhRepo.Verify(r => r.Create());
                else
                    _efRepo.Verify(r => r.Create());
            }

            [Test]
            public void InsertNewCustomerIfItDoesNotExistAtDestination()
            {
                WorkflowInvoker.Invoke(new SyncChange(), _inputs);
                _mockRepo.Verify(r => r.Add(It.Is((Customer c) => c.Email == "a@a.com")));
                _mockRepo.Verify(r => r.Save());
            }

            [Test]
            public void UpdateCustomerIfItExistsAtDestination()
            {
                var customer = new Customer {Email = "a@a.com"};
                _customers.Add(customer);
                WorkflowInvoker.Invoke(new SyncChange(), _inputs);
                _mockRepo.Verify(r => r.Update(customer));
                _mockRepo.Verify(r => r.Save());
            }
        }
    }
}
