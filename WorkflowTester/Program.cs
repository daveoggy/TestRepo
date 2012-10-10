using System;
using System.Activities;
using MegaProject.Workflow.Activities;

namespace WorkflowTester
{

    class Program
    {
        static void Main(string[] args)
        {
            WorkflowInvoker.Invoke(new Main());
            Console.WriteLine("That's it!");
            Console.ReadLine();
        }
    }
}
