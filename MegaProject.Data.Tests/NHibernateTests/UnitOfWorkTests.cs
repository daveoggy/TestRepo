using System;
using System.Data;
using MegaProject.Data.NHibernate;
using Moq;
using NHibernate;
using NUnit.Framework;

namespace MegaProject.Data.Tests.NHibernateTests
{
    public class UnitOfWorkTests
    {
        [TestFixture]
        public class NHibernateUnitOfWork_Should
        {
            private Mock<ISessionFactory> _mockFactory;
            private Mock<ISession> _mockSession;
            private Mock<ITransaction> _mockTransaction;
            private Mock<INHUnitOfWork> _mockUow;

            [SetUp]
            public void Setup()
            {
                _mockFactory = new Mock<ISessionFactory>();
                _mockSession = new Mock<ISession>();
                _mockTransaction = new Mock<ITransaction>();
                _mockUow = new Mock<INHUnitOfWork>();

                _mockUow.SetupGet(u => u.Session).Returns(_mockSession.Object);
                _mockFactory.Setup(mf => mf.GetCurrentSession()).Returns(_mockSession.Object);
                _mockFactory.Setup(mf => mf.OpenSession()).Returns(_mockSession.Object);
                _mockSession.Setup(ms => ms.BeginTransaction()).Returns(_mockTransaction.Object);
                _mockSession.Setup(ms => ms.BeginTransaction(It.IsAny<IsolationLevel>())).Returns(_mockTransaction.Object);
                _mockTransaction.Setup(mt => mt.Commit());

//                _mockSession.Setup(ms => ms.Delete(It.IsAny<Customer>()));
//                _mockSession.Setup(ms => ms.Update(It.IsAny<Customer>()));
//                _mockSession.Setup(ms => ms.Save(It.IsAny<Customer>()));
//                _mockSession.Setup(ms => ms.Get<Customer>(It.IsAny<int>())).Returns((int id) => new Customer { Int32Id = id });

                //                var queryble = new Mock<IQueryable<Customer>>();
                //                _mockSession.Setup(ms => ms.Query<Customer>()).Returns(queryble.Object);
            }
            
            [Test]
            public void Use_IsolationLevel_Of_ReadCommitted_By_Default()
            {
                var uow = new NHibernateUnitOfWork(_mockSession.Object);

                _mockSession.Verify(ms => ms.BeginTransaction(IsolationLevel.ReadCommitted));
                uow.Dispose();
            }

            [Test]
            [ExpectedException(typeof(ArgumentNullException))]
            public void Throw_If_Session_Is_Null()
            {
                var uow = new NHibernateUnitOfWork(null);
            }

            [Test]
            public void Save_Session_For_Reuse()
            {
                var uow = new NHibernateUnitOfWork(_mockSession.Object);

                Assert.AreSame(uow.Session, _mockSession.Object);
                uow.Dispose();
            }

            [Test]
            public void Open_A_New_Transaction()
            {
                var uow = new NHibernateUnitOfWork(_mockSession.Object, IsolationLevel.ReadUncommitted);

                _mockSession.Verify(ms => ms.BeginTransaction(It.IsAny<IsolationLevel>()));
                uow.Dispose();
            }

            [Test]
            public void Commit_Transaction_When_Called()
            {
                using (var uow = new NHibernateUnitOfWork(_mockSession.Object, IsolationLevel.ReadUncommitted))
                {
                    uow.Commit();
                    _mockTransaction.Verify(mt => mt.Commit());
                }
            }
        }
    }
}
