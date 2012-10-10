using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using MegaProject.Data.Entities;
using MegaProject.Data.NHibernate;
using MegaProject.Data.NHibernate.Helpers;
using Moq;
using NHibernate;
using NHibernate.Linq;
using NUnit.Framework;

namespace MegaProject.Data.Tests.NHibernateTests
{
    public class RepositoryTests
    {
        
        [TestFixture]
        public class Repository_Should
        {
            private Mock<ISessionFactory> _mockFactory;
            private Mock<ISession> _mockSession;
            private Mock<ITransaction> _mockTransaction;
            private Mock<INHUnitOfWork> _mockUow;

            private ISessionFactory SetHelperFactory(ISessionFactory factory)
            {
                // have to use reflection here to overcome static classes untestability
                var fieldInfo = typeof(NHibernateHelper).GetField("_factory",
                                                                   BindingFlags.Static | BindingFlags.NonPublic |
                                                                   BindingFlags.SetField);
                //save old value of factory for other tests to work properly
                var oldValue = fieldInfo.GetValue(null);
                fieldInfo.SetValue(null, factory);

                return oldValue as ISessionFactory;
            }

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

                _mockSession.Setup(ms => ms.Delete(It.IsAny<Customer>()));
                _mockSession.Setup(ms => ms.Update(It.IsAny<Customer>()));
                _mockSession.Setup(ms => ms.Save(It.IsAny<Customer>()));
                _mockSession.Setup(ms => ms.Get<Customer>(It.IsAny<int>())).Returns((int id) => new Customer {Int32Id = id});

//                var queryble = new Mock<IQueryable<Customer>>();
//                _mockSession.Setup(ms => ms.Query<Customer>()).Returns(queryble.Object);
            }
            
            [Test]
            [ExpectedException(typeof(ArgumentNullException))]
            public void Throw_If_Passed_Null_Unit_Of_Work()
            {
                var repo = new Repository((INHUnitOfWork) null);
            }

            [Test]
            [ExpectedException(typeof(ArgumentNullException))]
            public void Throw_If_Passed_Null_ISession()
            {
                var repo = new Repository((ISession)null);
            }
            
            [Test]
            public void Use_Session_From_Unit_Of_Work()
            {
                var repo = new Repository(_mockUow.Object);

                _mockUow.VerifyGet(u => u.Session);
            }

            [Test]
            public void Use_Current_Session_If_Available()
            {
                var oldValue = SetHelperFactory(_mockFactory.Object);
               
                var repo = new Repository();

                _mockFactory.Verify(mf => mf.GetCurrentSession());

                // restore old value of factory for other tests to work properly
                SetHelperFactory(oldValue);
                repo.Dispose();
            }

            [Test]
            [Ignore("CurrentSessionContext not mocked")]
            public void Create_New_Session_If_No_Current_Session_Available()
            {
                _mockFactory.Setup(mf => mf.GetCurrentSession()).Throws(new HibernateException());

                var oldValue = SetHelperFactory(_mockFactory.Object);

                var repo = new Repository();

                _mockFactory.Verify(mf => mf.OpenSession());
                _mockSession.Verify(ms => ms.BeginTransaction());

                SetHelperFactory(oldValue);
                repo.Dispose();
            }

            [Test]
            [Ignore("CurrentSessionContext not mocked")]
            public void Commit_Transaction_When_Owning_Session()
            {
                _mockFactory.Setup(mf => mf.GetCurrentSession()).Throws(new HibernateException());

                var oldValue = SetHelperFactory(_mockFactory.Object);

                var repo = new Repository();
                repo.Save();

                _mockTransaction.Verify(mt => mt.Commit());

                SetHelperFactory(oldValue);
                repo.Dispose();
            }

            [Test]
            public void Be_Able_To_Delete_Entity()
            {
                var oldValue = SetHelperFactory(_mockFactory.Object);

                var repo = new Repository();
                var cust = new Customer {Int32Id = 4};
                
                repo.Delete(cust);
                
                _mockSession.Verify(ms => ms.Delete(cust));

                SetHelperFactory(oldValue);
                repo.Dispose();
            }

            [Test]
            public void Be_Able_To_Update_Entity()
            {
                var oldValue = SetHelperFactory(_mockFactory.Object);

                var repo = new Repository();
                var cust = new Customer { Int32Id = 4 };

                repo.Update(cust);

                _mockSession.Verify(ms => ms.Update(cust));

                SetHelperFactory(oldValue);
                repo.Dispose();
            }

            [Test]
            public void Be_Able_To_Add_New_Entity()
            {
                var oldValue = SetHelperFactory(_mockFactory.Object);

                var repo = new Repository();
                var cust = new Customer { Int32Id = 4 };

                repo.Add(cust);

                _mockSession.Verify(ms => ms.Save(cust));

                SetHelperFactory(oldValue);
                repo.Dispose();
            }

            [Test]
            public void Be_Able_To_Add_Multiple_New_Entities()
            {
                var oldValue = SetHelperFactory(_mockFactory.Object);

                var repo = new Repository();
                IEnumerable<Customer> customers = new[]
                                    {
                                        new Customer{ Int32Id = 4 },
                                        new Customer{ Int32Id = 36 },
                                        new Customer{ Int32Id = 90 }
                                    };

                repo.Add(customers);

                _mockSession.Verify(ms => ms.Save(It.IsAny<Customer>()), Times.Exactly(3));

                SetHelperFactory(oldValue);
                repo.Dispose();
            }

            [Test]
            [ExpectedException(typeof(NotImplementedException))]
            public void Throw_When_Asked_To_Get_Entity_By_Guid()
            {
                var oldValue = SetHelperFactory(_mockFactory.Object);

                var repo = new Repository();
                try
                {
                    repo.GetById<Customer>(Guid.NewGuid());
                }
                finally
                {
                    SetHelperFactory(oldValue);
                    repo.Dispose();
                }
            }

            [Test]
            public void Be_Able_To_Get_Entity_By_Id()
            {
                var oldValue = SetHelperFactory(_mockFactory.Object);

                var repo = new Repository();

                repo.GetById<Customer>(4);

                _mockSession.Verify(ms => ms.Get<Customer>(4));

                SetHelperFactory(oldValue);
                repo.Dispose();
            }

            [Test]
            [Ignore("Cannot yet mock extension methods for NHibernate LINQ support")]
            public void Be_Able_To_Query_All_Entities()
            {
                var oldValue = SetHelperFactory(_mockFactory.Object);

                var repo = new Repository();

                repo.All<Customer>();

                _mockSession.Verify(ms => ms.Query<Customer>());

                SetHelperFactory(oldValue);
                repo.Dispose();
            }
        }
    }
}
