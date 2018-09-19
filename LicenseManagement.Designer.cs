namespace Simulation_Options
{
	partial class LicenseManagement
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LicenseManagement));
			this.btnInstallNetworkLicenseServer = new System.Windows.Forms.Button();
			this.btnReactivateLicense = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnUpdates = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnInstallNetworkLicenseServer
			// 
			resources.ApplyResources(this.btnInstallNetworkLicenseServer, "btnInstallNetworkLicenseServer");
			this.btnInstallNetworkLicenseServer.Name = "btnInstallNetworkLicenseServer";
			this.btnInstallNetworkLicenseServer.UseVisualStyleBackColor = true;
			this.btnInstallNetworkLicenseServer.Click += new System.EventHandler(this.btnInstallNetworkLicenseServer_Click);
			// 
			// btnReactivateLicense
			// 
			resources.ApplyResources(this.btnReactivateLicense, "btnReactivateLicense");
			this.btnReactivateLicense.Name = "btnReactivateLicense";
			this.btnReactivateLicense.UseVisualStyleBackColor = true;
			this.btnReactivateLicense.Click += new System.EventHandler(this.btnReactivateLicense_Click);
			// 
			// btnCancel
			// 
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnUpdates
			// 
			resources.ApplyResources(this.btnUpdates, "btnUpdates");
			this.btnUpdates.Name = "btnUpdates";
			this.btnUpdates.UseVisualStyleBackColor = true;
			this.btnUpdates.Click += new System.EventHandler(this.btnUpdates_Click);
			// 
			// LicenseManagement
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btnUpdates);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnInstallNetworkLicenseServer);
			this.Controls.Add(this.btnReactivateLicense);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LicenseManagement";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnInstallNetworkLicenseServer;
		private System.Windows.Forms.Button btnReactivateLicense;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnUpdates;
	}
}