using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using MegaProject.Data.Entities;
using Microsoft.Practices.Unity;
using log4net;
using NH = MegaProject.Data.NHibernate.Helpers;
using EF = MegaProject.Data.EntityFramework.Helpers;

namespace MegaProject.Workflow.Activities
{

    public sealed class GetChanges : CodeActivity<IList<CustomerAudit>>
    {
        public InArgument<IUnityContainer> Container { get; set; }
        public InArgument<ChangeSource> Source { get; set; }

        protected override IList<CustomerAudit> Execute(CodeActivityContext context)
        {
            var container = context.GetValue(this.Container);
            var source = context.GetValue(this.Source);
            var logger = container.Resolve<ILog>();

            IList<CustomerAudit> changes = new List<CustomerAudit>();
            logger.InfoFormat("Getting changes for {0}", source);

            try
            {
                using (var repo =
                        (source == ChangeSource.MSSQL)
                        ? container.Resolve<EF.IRepositoryFactory>().Create()
                        : container.Resolve<NH.IRepositoryFactory>().Create())
                    changes = repo.All<CustomerAudit>().Where(ca => !ca.IsSynced).ToList();

                foreach (var change in changes)
                    change.Source = source;

                logger.InfoFormat("Changes for {0}: got {1} records", source, changes.Count);
            }
            catch (Exception ex)
            {
                logger.Error("Error while trying to get pending changes for " + source, ex);
                throw;
            }

            return changes;
        }
    }
}
