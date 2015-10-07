namespace SiteSurveyor
{
	partial class Installer
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.SiteSurveyorProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
			this.SiteSurveyorServiceInstaller = new System.ServiceProcess.ServiceInstaller();
			// 
			// SiteSurveyorProcessInstaller
			// 
			this.SiteSurveyorProcessInstaller.Account = System.ServiceProcess.ServiceAccount.NetworkService;
			this.SiteSurveyorProcessInstaller.Password = null;
			this.SiteSurveyorProcessInstaller.Username = null;
			// 
			// SiteSurveyorServiceInstaller
			// 
			this.SiteSurveyorServiceInstaller.DelayedAutoStart = true;
			this.SiteSurveyorServiceInstaller.Description = "Monitor Web Site";
			this.SiteSurveyorServiceInstaller.DisplayName = "WebSiteSurveyor";
			this.SiteSurveyorServiceInstaller.ServiceName = "SiteSurveyor";
			this.SiteSurveyorServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
			// 
			// Installer
			// 
			this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.SiteSurveyorProcessInstaller,
            this.SiteSurveyorServiceInstaller});

		}

		#endregion

		private System.ServiceProcess.ServiceProcessInstaller SiteSurveyorProcessInstaller;
		private System.ServiceProcess.ServiceInstaller SiteSurveyorServiceInstaller;
	}
}