using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace MegaProject.WindowsService
{
    partial class MegaService : ServiceBase
    {
        private IScheduler _scheduler;

        public MegaService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            StartMonitoring();
        }

        private void StartMonitoring()
        {
            ISchedulerFactory factory = new StdSchedulerFactory();
            _scheduler = factory.GetScheduler();

            _scheduler.Start();

            IJobDetail jobDetail = JobBuilder.Create(typeof (MegaJob))
                .WithIdentity("MegaJob")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("MegaTrigger")
                .StartAt(DateBuilder.FutureDate(2, IntervalUnit.Minute))
                .WithSimpleSchedule(b => b.WithIntervalInMinutes(2))
                .Build();

            _scheduler.ScheduleJob(jobDetail, trigger);
        }

        protected override void OnStop()
        {
            _scheduler.Shutdown(true);
        }

        protected override void OnPause()
        {
            _scheduler.Standby();
        }

        protected override void OnContinue()
        {
            _scheduler.ResumeAll();
        }
    }
}
