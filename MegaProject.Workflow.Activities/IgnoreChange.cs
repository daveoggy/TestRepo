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

    public sealed class IgnoreChange : CodeActivity
    {
        public InArgument<IUnityContainer> Container { get; set; }
        public InArgument<CustomerAudit> Ignored { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var container = context.GetValue(this.Container);
            var ignored = context.GetValue(this.Ignored);
            var logger = container.Resolve<ILog>();

            logger.InfoFormat("Attempting to mark ignored change to {0} as synced", ignored.Email);

            try
            {
                ignored.IsSynced = true;

                using (var repo =
                    (ignored.Source == ChangeSource.MSSQL)
                    ? container.Resolve<EF.IRepositoryFactory>().Create()
                    : container.Resolve<NH.IRepositoryFactory>().Create())
                {
                    var change = repo.All<CustomerAudit>().SingleOrDefault(ca => ca.Id == ignored.Id);
                    if (change != null)
                    {
                        change.IsSynced = true;
                        repo.Update(change);
                        repo.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error while trying to mark the change as synced.", ex);
                throw;
            }

            logger.InfoFormat("Succesfully ignored change to {0}", ignored.Email);
        }
    }
}
