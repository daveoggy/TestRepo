using MegaProject.Data.NHibernate.Helpers;
using NUnit.Framework;

namespace MegaProject.Data.Tests.NHibernateTests
{
    
    [TestFixture]
    public class HelpersTests
    {
        [Test]
        public void NHibernateHelper_Should_Maintain_Only_One_SessionFactory()
        {
            var factory1 = NHibernateHelper.Factory;
            var factory2 = NHibernateHelper.Factory;
            Assert.AreSame(factory1, factory2);
        }

        [Test]
        public void RepositoryFactory_Should_Return_New_Repository_Every_Time()
        {
            var factory = new RepositoryFactory();
            var repo1 = factory.Create();
            var repo2 = factory.Create();
            Assert.AreNotSame(repo1, repo2);
            repo2.Dispose();
            repo1.Dispose();
        }

        
        [Test]
        public void MegaNHibernateUnitOfWorkFactory_Should_Create_New_Instances_Of_Unit_Of_Work()
        {
            var factory = new MegaNHibernateUnitOfWorkFactory();
            var uow1 = factory.Create();
            var uow2 = factory.Create();
            Assert.AreNotSame(uow1, uow2);
            uow2.Dispose();
            uow1.Dispose();
        }
    }
}
