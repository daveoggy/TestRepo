using System.Activities;
using MegaProject.Utilities.Unity;
using Microsoft.Practices.Unity;
using log4net;
using EF = MegaProject.Data.EntityFramework.Helpers;
using NH = MegaProject.Data.NHibernate.Helpers;

namespace MegaProject.Workflow.Activities
{

    public sealed class SetupContainer : CodeActivity<IUnityContainer>
    {
        protected override IUnityContainer Execute(CodeActivityContext context)
        {
            log4net.Config.XmlConfigurator.Configure();

            IUnityContainer container = new UnityContainer()
                .AddNewExtension<BuildTracking>()
                .AddNewExtension<LogCreation>()
                .RegisterType<EF.IRepositoryFactory, EF.RepositoryFactory>()
                .RegisterType<NH.IRepositoryFactory, NH.RepositoryFactory>();

            var logger = container.Resolve<ILog>();
            logger.Info("-----------------------------------------------");
            logger.Info("Workflow started");
            logger.Info("Configured Unity container");

            return container;
        }
    }
}
