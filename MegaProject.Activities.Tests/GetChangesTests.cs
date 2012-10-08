using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using MegaProject.Data.Contracts;
using MegaProject.Data.Entities;
using MegaProject.Workflow.Activities;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;
using log4net;
using NH = MegaProject.Data.NHibernate.Helpers;
using EF = MegaProject.Data.EntityFramework.Helpers;

namespace MegaProject.Activities.Tests
{
    public class GetChangesTests
    {
        [TestFixture]
        public class GetChangesShould
        {
            private IUnityContainer _container;
            private List<CustomerAudit> _changes;
            private Mock<IRepositoryBase> _mockRepo;
            private Mock<ILog> _mockLog;
            private Mock<EF.IRepositoryFactory> _efRepo;
            private Mock<NH.IRepositoryFactory> _nhRepo;

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
                                           Added = stamp
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

                _efRepo = new Mock<EF.IRepositoryFactory>();
                _nhRepo = new Mock<NH.IRepositoryFactory>();

                _efRepo.Setup(f => f.Create()).Returns(_mockRepo.Object);
                _nhRepo.Setup(f => f.Create()).Returns(_mockRepo.Object);

                _container.RegisterInstance(typeof (EF.IRepositoryFactory), _efRepo.Object);
                _container.RegisterInstance(typeof (NH.IRepositoryFactory), _nhRepo.Object);
                _container.RegisterInstance(typeof (ILog), _mockLog.Object);
            }
            
            [TestCase(ChangeSource.MSSQL)]
            [TestCase(ChangeSource.Oracle)]
            public void SetTheCorrectChangeSource(ChangeSource source)
            {
                IDictionary<string, object> inputs = new Dictionary<string, object>
                {
                    {"Container", _container},
                    {"Source", source}
                };

                WorkflowInvoker.Invoke(new GetChanges(), inputs);
                _mockRepo.Verify(r => r.All<CustomerAudit>());
                Assert.AreEqual(source, _changes[0].Source);
                Assert.AreEqual(source, _changes[1].Source);
            }

            [TestCase(ChangeSource.MSSQL)]
            [TestCase(ChangeSource.Oracle)]
            public void CreateTheCorrectRepositoryType(ChangeSource source)
            {
                IDictionary<string, object> inputs = new Dictionary<string, object>
                {
                    {"Container", _container},
                    {"Source", source}
                };

                WorkflowInvoker.Invoke(new GetChanges(), inputs);
                if(source == ChangeSource.MSSQL)
                    _efRepo.Verify(f => f.Create());
                else
                    _nhRepo.Verify(f => f.Create());
            }
        }
    }
}
