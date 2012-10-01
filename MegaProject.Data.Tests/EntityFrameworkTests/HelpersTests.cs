using MegaProject.Data.EntityFramework.Helpers;
using NUnit.Framework;

namespace MegaProject.Data.Tests.EntityFrameworkTests
{
    public class HelpersTests
    {
        [TestFixture]
        //public class RepositoryFactoriesShould
        //{
        //    [Test]
        //    public void ReturnDefaultImplementationWhenNotOverriden()
        //    {
        //        var fact = new RepositoryFactory();

        //        var result = fact.GetRepositoryFactoryForEntityType<Customer>();
        //        Assert.IsNotNull(result);
        //        Assert.IsInstanceOf<Func<EntityFrameworkUnitOfWork, object>>(result);
        //    }

        //    [Test]
        //    public void ReturnOverridenImplementationWhenConfigured()
        //    {
        //        Func<EntityFrameworkUnitOfWork, object> factory = uow => new EntityFrameworkRepository<Customer>(uow);
        //        var dict = new Dictionary<Type, Func<EntityFrameworkUnitOfWork, object>> { { typeof(Customer), factory } };
        //        var fact = new RepositoryFactory(dict);

        //        var result = fact.GetRepositoryFactoryForEntityType<Customer>();
        //        Assert.IsNotNull(result);
        //        Assert.AreSame(factory, result);
        //    }
        //}

       // [TestFixture]
        //public class DefaultEntityFrameworkRepositoryProviderShould
        //{
        //    [Test]
        //    [ExpectedException(typeof(NotImplementedException))]
        //    public void ThrowIfFactoriesDoesNotReturnAFactory()
        //    {
        //        var mockFactories = new Mock<RepositoryFactory>();
        //        mockFactories.Setup(x => x.GetRepositoryFactory<Customer>()).Returns(() => null);
        //        var provider = new DefaultEntityFrameworkRepositoryProvider(mockFactories.Object);
        //        provider.GetRepository<Customer>();
        //    }

        //    [Test]
        //    public void AllowSavingRepositoryToCache()
        //    {
        //        var mockRepository = new Mock<IRepository<Customer>>();
        //        var mockFactories = new Mock<RepositoryFactory>();
        //        var provider = new DefaultEntityFrameworkRepositoryProvider(mockFactories.Object);
        //        Assert.DoesNotThrow(() => provider.SetRepository(mockRepository));
        //    }

        //    [Test]
        //    public void RetrieveRepositoryFromCacheIfPresent()
        //    {
        //        var mockRepository = new object();
        //        var mockFactories = new Mock<RepositoryFactory>();
        //        var provider = new DefaultEntityFrameworkRepositoryProvider(mockFactories.Object);

        //        provider.SetRepository(mockRepository);
        //        var actual = provider.GetRepository<object>();
        //        Assert.AreSame(mockRepository, actual);
        //    }

        //    [Test]
        //    public void MakeANewRepositoryAndCacheItIfNoValidRepositoryIsFound()
        //    {
        //        var mockRepository = new Mock<IRepository<Customer>>();
        //        Func<DbContext, object> mockFactory = dbContext => mockRepository.Object;
        //        var mockFactories = new Mock<RepositoryFactory>();
        //        mockFactories.Setup(x => x.GetRepositoryFactoryForEntityType<Customer>()).Returns(() => mockFactory);
        //        var provider = new DefaultEntityFrameworkRepositoryProvider(mockFactories.Object);

        //        var first = provider.GetRepositoryForEntityType<Customer>();
        //        var second = provider.GetRepositoryForEntityType<Customer>();
        //        Assert.IsNotNull(first);
        //        Assert.AreSame(first,second);
        //    }
        //}

        [TestFixture]
        public class DefaultEntityFrameworkUnitOfWorkFactoryShould
        {
            [Test]
            public void CreateANewUnitOfWorkEveryTime()
            {
                var factory = new DefaultEntityFrameworkUnitOfWorkFactory();
                var firstUoW = factory.Create();
                var secondUoW = factory.Create();
                Assert.IsNotNull(firstUoW);
                Assert.IsNotNull(secondUoW);
                Assert.AreNotSame(firstUoW,secondUoW);

            }
        }
    }
}
