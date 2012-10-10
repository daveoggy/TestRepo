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
    public class MarkSyncedTests
    {
        [TestFixture]
        public class MarkSyncedShould
        {
            private IUnityContainer _container;
            private Mock<IRepositoryBase> _mockRepoEF;
            private Mock<IRepositoryBase> _mockRepoNH;
            private Mock<ILog> _mockLog;
            private Mock<Data.EntityFramework.Helpers.IRepositoryFactory> _efRepo;
            private Mock<Data.NHibernate.Helpers.IRepositoryFactory> _nhRepo;
            private List<CustomerAudit> _changesEF;
            private List<CustomerAudit> _changesNH
                ;

            private DateTime _stamp;
            private CustomerAudit _change;
            private IDictionary<string, object> _inputs;

            [SetUp]
            public void Setup()
            {
                _container = new UnityContainer();
                _mockLog = new Mock<ILog>();
                _changesEF = new List<CustomerAudit>();
                _changesNH = new List<CustomerAudit>();
                _mockRepoEF = new Mock<IRepositoryBase>();
                _mockRepoNH = new Mock<IRepositoryBase>();
                _mockRepoEF.Setup(r => r.All<CustomerAudit>()).Returns(_changesEF.AsQueryable());
                _mockRepoNH.Setup(r => r.All<CustomerAudit>()).Returns(_changesNH.AsQueryable());

                _efRepo = new Mock<Data.EntityFramework.Helpers.IRepositoryFactory>();
                _nhRepo = new Mock<Data.NHibernate.Helpers.IRepositoryFactory>();

                _efRepo.Setup(f => f.Create()).Returns(_mockRepoEF.Object);
                _nhRepo.Setup(f => f.Create()).Returns(_mockRepoNH.Object);

                _container.RegisterInstance(typeof(Data.EntityFramework.Helpers.IRepositoryFactory), _efRepo.Object);
                _container.RegisterInstance(typeof(Data.NHibernate.Helpers.IRepositoryFactory), _nhRepo.Object);
                _container.RegisterInstance(typeof(ILog), _mockLog.Object);


                _stamp = new DateTime(2012, 10, 15, 15, 15, 15);
                _change = new CustomerAudit
                {
                    Email = "a@a.com",
                    IsSynced = false,
                    Type = "insert",
                    Added = _stamp,
                    Source = ChangeSource.MSSQL
                };

                _inputs = new Dictionary<string, object>
                {
                    {"Container", _container},
                    {"Change", _change}
                };
            }

            [Test]
            public void UpdateBothDataSources()
            {
                var change2 = new CustomerAudit
                {
                    Email = "a@a.com",
                    IsSynced = false,
                    Type = "insert",
                    Added = _stamp.AddSeconds(5),
                    Source = ChangeSource.MSSQL
                };
                var change3 = new CustomerAudit
                {
                    Email = "a@a.com",
                    IsSynced = false,
                    Type = "insert",
                    Added = _stamp.AddSeconds(5),
                    Source = ChangeSource.Oracle
                };
                _changesEF.Add(change2);
                _changesNH.Add(change3);

                WorkflowInvoker.Invoke(new MarkSynced(), _inputs);

                _efRepo.Verify(f => f.Create());
                _nhRepo.Verify(f => f.Create());
                _mockRepoEF.Verify(r => r.Save());
                _mockRepoNH.Verify(r => r.Save());
            }

            [TestCase(ChangeSource.MSSQL)]
            [TestCase(ChangeSource.Oracle)]
            public void SetBothSidesAsSynced(ChangeSource source)
            {
                _change.Source = source;
                var change2 = new CustomerAudit
                {
                    Email = "a@a.com",
                    IsSynced = false,
                    Type = "insert",
                    Added = _stamp.AddSeconds(5),
                    Source = ChangeSource.Oracle
                };
                _changesNH.Add(change2);
                var change3 = new CustomerAudit
                {
                    Email = "a@a.com",
                    IsSynced = false,
                    Type = "insert",
                    Added = _stamp.AddSeconds(7),
                    Source = ChangeSource.Oracle
                };
                _changesEF.Add(change3);

                WorkflowInvoker.Invoke(new MarkSynced(), _inputs);

                _mockRepoEF.Verify(r => r.Update(It.Is((CustomerAudit ca) => ca.IsSynced)));
                _mockRepoNH.Verify(r => r.Update(It.Is((CustomerAudit ca) => ca.IsSynced)));

                Assert.IsTrue(change2.IsSynced || change3.IsSynced);
                Assert.IsTrue(_change.IsSynced);

                _mockRepoEF.Verify(r => r.Save());
                _mockRepoNH.Verify(r => r.Save());
            }
        }
    }
}
