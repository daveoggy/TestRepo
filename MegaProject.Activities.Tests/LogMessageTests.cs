using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MegaProject.Workflow.Activities;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;
using log4net;

namespace MegaProject.Activities.Tests
{
    public class LogMessageTests
    {
        [TestFixture]
        public class LogMessageShould
        {
            [TestCase("Info")]
            [TestCase("Error")]
            [TestCase("Warn")]
            public void CallTheCorrectMethodForLogging(string level)
            {
                var container = new UnityContainer();
                var logger = new Mock<ILog>();
                container.RegisterInstance(typeof (ILog), logger.Object);

                IDictionary<string, object> inputs = new Dictionary<string, object>
                {
                    {"Container", container},
                    {"Message", "test"},
                    {"Level", level}
                };

                WorkflowInvoker.Invoke(new LogMessage(), inputs);
                switch(level)
                {
                    case "Info":
                        logger.Verify(l => l.Info("test"));
                        break;
                    case "Error":
                        logger.Verify(l => l.Error("test"));
                        break;
                    case "Warn":
                        logger.Verify(l => l.Warn("test"));
                        break;
                }
            }
        }
    }
}
