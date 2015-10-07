using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace SiteSurveyor
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
			if (System.Environment.UserInteractive)
			{
				Console.WriteLine("starting SiteSurveyor");

				string parameter = string.Concat(args);
				switch (parameter)
				{
					case "/install":
						ServiceControllerHelper.InstallService("SiteSurveyor");
						break;
					case "/uninstall":
						ServiceControllerHelper.UninstallService("SiteSurveyor");
						break;
					default:
						System.Diagnostics.Trace.WriteLine("console mode detected");

						var svc = new MainService();
						svc.Initialize();
						svc.Start();

						System.Console.Read();

						svc.Stop();
						break;
				}
			}
			else
			{
				ServiceBase[] ServicesToRun;
				ServicesToRun = new ServiceBase[]
				{
					new MainService()
				};
				ServiceBase.Run(ServicesToRun);
			}
		}
	}
}
