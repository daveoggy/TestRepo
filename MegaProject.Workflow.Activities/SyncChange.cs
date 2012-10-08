using System;
using System.Activities;
using System.Linq;
using MegaProject.Data.Entities;
using MegaProject.Utilities;
using Microsoft.Practices.Unity;
using log4net;
using NH = MegaProject.Data.NHibernate.Helpers;
using EF = MegaProject.Data.EntityFramework.Helpers;

namespace MegaProject.Workflow.Activities
{

    public sealed class SyncChange : CodeActivity
    {
        public InArgument<CustomerAudit> Change { get; set; }
        public InArgument<IUnityContainer> Container { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var change = context.GetValue(this.Change);
            var container = context.GetValue(this.Container);
            var logger = container.Resolve<ILog>();

            logger.InfoFormat("Attempting to sync {0} of customer {1}. Source of change: {2}", change.Type, change.Email, change.Source);

            try
            {
                using (var repo =
                        (change.Source == ChangeSource.MSSQL)
                        ? container.Resolve<NH.IRepositoryFactory>().Create()
                        : container.Resolve<EF.IRepositoryFactory>().Create())
                {
                    var customer = repo.All<Customer>().SingleOrDefault(c => c.Email == change.Email);
                    if (customer == null)
                    {
                        customer = new Customer
                        {
                            Email = change.Email
                        };
                        customer.Update(change);
                        repo.Add(customer);
                    }
                    else
                    {
                        customer.Update(change);
                        repo.Update(customer);
                    }
                    repo.Save();
                    logger.InfoFormat("Changes to {0} saved.", customer.Email);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error while trying to sync a customer.", ex);
                throw;
            }
        }
    }
}
