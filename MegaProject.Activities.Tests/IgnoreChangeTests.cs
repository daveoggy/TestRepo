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
    public class IgnoreChangeTests
    {
        [TestFixture]
        public class IgnoreChangeShould
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
                var stamp = new DateTime(2012, 10, 15, 15, 15, 15);
                _mockLog = new Mock<ILog>();
                _changes = new List<CustomerAudit>
                               {
                                   new CustomerAudit
                                       {
                                           Email = "a@a.com",
                                           IsSynced = false,
                                           Type = "insert",
                                           Added = stamp,
                                           Id = 3
                                       },
                                   new CustomerAudit
                                       {
                                           Email = "b@b.com",
                                           IsSynced = false,
                                           Type = "update",
                                           Added = stamp.AddSeconds(20)
                                       }
                               };

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

            [TestCase(ChangeSource.MSSQL)]
            [TestCase(ChangeSource.Oracle)]
            public void CreateTheCorrectRepositoryType(ChangeSource source)
            {
                var change = new CustomerAudit { Id = 3, Source = source };
                
                IDictionary<string, object> inputs = new Dictionary<string, object>
                {
                    {"Container", _container},
                    {"Ignored", change}
                };

                WorkflowInvoker.Invoke(new IgnoreChange(), inputs);
                if (source == ChangeSource.MSSQL)
                    _efRepo.Verify(f => f.Create());
                else
                    _nhRepo.Verify(f => f.Create());
            }
        }
    }
}
