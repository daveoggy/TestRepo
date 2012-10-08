using System;
using System.Activities;
using System.Linq;
using MegaProject.Data.Entities;
using Microsoft.Practices.Unity;
using log4net;
using NH = MegaProject.Data.NHibernate.Helpers;
using EF = MegaProject.Data.EntityFramework.Helpers;

namespace MegaProject.Workflow.Activities
{

    public sealed class MarkSynced : CodeActivity
    {
        public InArgument<IUnityContainer> Container { get; set; }
        public InArgument<CustomerAudit> Change { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var container = context.GetValue(this.Container);
            var change = context.GetValue(this.Change);
            var logger = container.Resolve<ILog>();

            logger.InfoFormat("Trying to mark the change to {0} at {1} as synced.", change.Email, change.Added);

            change.IsSynced = true;

            try
            {
                using (var scope = new System.Transactions.TransactionScope())
                {
                    using (var repoOracle = container.Resolve<NH.IRepositoryFactory>().Create())
                    using (var repoSql = container.Resolve<EF.IRepositoryFactory>().Create())
                    {
                        switch (change.Source)
                        {
                            case ChangeSource.MSSQL:
                                repoSql.Update(change);
                                var extraOracle = repoOracle.All<CustomerAudit>().Single(ca =>
                                                                                   (ca.Email == change.Email) &&
                                                                                   (ca.Added > change.Added));
                                extraOracle.IsSynced = true;
                                repoOracle.Update(extraOracle);
                                break;
                            case ChangeSource.Oracle:
                                repoOracle.Update(change);
                                var extraSql = repoSql.All<CustomerAudit>().Single(ca =>
                                                                                   (ca.Email == change.Email) &&
                                                                                   (ca.Added > change.Added));
                                extraSql.IsSynced = true;
                                repoSql.Update(extraSql);
                                break;
                        }
                        repoOracle.Save();
                        repoSql.Save();
                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error while trying to mark the change as synced.", ex);
                throw;
            }
        }
    }
}
