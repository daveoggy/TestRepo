using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MegaProject.Workflow.Activities;
using Quartz;

namespace MegaProject.WindowsService
{
    public class MegaJob : IJob
    {
        public MegaJob()
        {
            
        }
        
        public void Execute(IJobExecutionContext context)
        {
            WorkflowInvoker.Invoke(new Main());
        }
    }
}
