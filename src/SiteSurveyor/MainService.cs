using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

using TasksOnTime.Scheduling;

namespace SiteSurveyor
{
	public partial class MainService : ServiceBase
	{
		public MainService()
		{
			InitializeComponent();
		}

		public void Initialize()
		{
			TasksOnTime.GlobalConfiguration.Settings.ScheduledTaskDisabledByDefault = false;
			var task = Scheduler.CreateScheduledTask<Service.SurveyorTask>("surveyor")
					.AllowMultipleInstance(false)
					.EveryMinute(1);

			Scheduler.Add(task);
		}

		public void Start()
		{
			Scheduler.Start();
		}

		protected override void OnStart(string[] args)
		{
			Initialize();
			Start();
		}

		protected override void OnStop()
		{
			Scheduler.Stop();
		}
	}
}
