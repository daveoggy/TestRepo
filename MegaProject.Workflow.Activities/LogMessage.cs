using System.Activities;
using Microsoft.Practices.Unity;
using log4net;

namespace MegaProject.Workflow.Activities
{

    public sealed class LogMessage : CodeActivity
    {
        // Define an activity input argument of type string
        public InArgument<string> Message { get; set; }
        public InArgument<string> Level { get; set; }
        public InArgument<IUnityContainer> Container { get; set; }

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(CodeActivityContext context)
        {
            var text = context.GetValue(this.Message);
            var level = context.GetValue(this.Level);
            var container = context.GetValue(this.Container);

            var logger = container.Resolve<ILog>();
            switch(level)
            {
                case "Warn":
                    logger.Warn(text);
                    break;
                case "Error":
                    logger.Error(text);
                    break;
                case "Fatal":
                    logger.Fatal(text);
                    break;
                default:
                    logger.Info(text);
                    break;
            }
            
        }
    }
}
