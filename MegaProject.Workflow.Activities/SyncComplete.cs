using System.Activities;
using System.Linq;
using MegaProject.Data.Entities;
using Microsoft.Practices.Unity;
using log4net;
using NH = MegaProject.Data.NHibernate.Helpers;
using EF = MegaProject.Data.EntityFramework.Helpers;

namespace MegaProject.Workflow.Activities
{

    public sealed class SyncComplete : CodeActivity<bool>
    {
        public InArgument<IUnityContainer> Container { get; set; }
        public InArgument<CustomerAudit> Change { get; set; }

        protected override bool Execute(CodeActivityContext context)
        {
            var container = context.GetValue(this.Container);
            var change = context.GetValue(this.Change);
            var logger = container.Resolve<ILog>();
            var result = false;

            logger.InfoFormat("Checking for a succesfull sync of customer: {0}", change.Email);

            using(var repo = 
                (change.Source == ChangeSource.MSSQL)
                ? container.Resolve<NH.IRepositoryFactory>().Create()
                : container.Resolve<EF.IRepositoryFactory>().Create())
                result = repo.All<CustomerAudit>().Any(ca =>
                                    (ca.Email == change.Email) && (ca.Added > change.Added));

            logger.InfoFormat("Check resulted in {0}", result?"success":"failure");

            return result;
        }
    }
}
