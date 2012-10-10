using System.Activities;
using System.Collections.Generic;
using System.Linq;
using MegaProject.Data.Entities;
using Microsoft.Practices.Unity;
using log4net;

namespace MegaProject.Workflow.Activities
{

    public sealed class MergeChanges : CodeActivity
    {
        public InArgument<IUnityContainer> Container { get; set; }
        public InArgument<IList<CustomerAudit>> OracleChanges { get; set; }
        public InArgument<IList<CustomerAudit>> MSSQLChanges { get; set; }

        public OutArgument<IList<CustomerAudit>> ToSync { get; set; }
        public OutArgument<IList<CustomerAudit>> Ignored { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var container = context.GetValue(this.Container);
            var changeOracle = context.GetValue(this.OracleChanges);
            var changesSql = context.GetValue(this.MSSQLChanges);
            var logger = container.Resolve<ILog>();

            logger.InfoFormat("Merging changes. Pending from MSSQL: {0}; from Oracle: {1}", changesSql.Count, changeOracle.Count);

            var all = changesSql.Concat(changeOracle);
            var actual = all.OrderByDescending(ca => ca.Added)
                .GroupBy(ca => ca.Email)
                .Select(g => g.First())
                .OrderBy(ca => ca.Added)
                .ToList();
            var ignored = all.Except(actual).ToList();

            logger.InfoFormat("Merge complete. Total changes to sync: {0}", actual.Count);
            logger.InfoFormat("Changes ignored: {0}", ignored.Count);

            this.ToSync.Set(context, actual);
            this.Ignored.Set(context, ignored);
        }
    }
}
