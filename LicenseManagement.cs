using System;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace Simulation_Options
{
	public partial class LicenseManagement : Form
	{
		public LicenseManagement()
		{
			InitializeComponent();
		}

		private void btnReactivateLicense_Click(object sender, EventArgs e)
		{
			var proc = new Process();
			string licensePath = Application.StartupPath;
			if (File.Exists(licensePath + "\\ReactivateLicense.exe"))
			{
				try
				{
					//proc.StartInfo.FileName = @"C:\WINDOWS\NOTEPAD.EXE";
					proc.StartInfo.FileName = Application.StartupPath + "\\ReactivateLicense.exe";
					proc.StartInfo.UseShellExecute = true;
					proc.StartInfo.RedirectStandardOutput = false;
					proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
					proc.Start();
				}
				catch (Exception ex)
				{
					MessageBox.Show(Universal.Idioma("ERROR 100618.0659: error reactivating RiverFlow2D license!. ", "ERROR 100618.0659: ¡error reactivando licencia de RiverFlow2D! ") + ex.Message,
						"RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			else
			{
				MessageBox.Show(Universal.Idioma("Reactivation of license of RiverFlow2D not found!", "¡No se encontró el reactivador de licencia de RiverFLow2D¡"),
					"RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

				var openFileDialog = new OpenFileDialog
				{
					Filter = "ReactivateLicense.exe (*.exe)|*.exe",
					FilterIndex = 1,
					Title = "Choose ReactivateLicense.exe",
					CheckFileExists = true,
					FileName = "ReactivateLicense.exe"
				};

				if (openFileDialog.ShowDialog() == DialogResult.OK)
					try
					{
						proc.StartInfo.FileName = openFileDialog.FileName;
						proc.StartInfo.UseShellExecute = true;
						proc.StartInfo.RedirectStandardOutput = false;
						proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
						proc.Start();
					}
					catch (Exception ex)
					{
						MessageBox.Show(Universal.Idioma("ERROR 100618.0659: error reactivating RiverFlow2D license!. ", "ERROR 100618.0659: ¡error reactivando licencia de RiverFlow2D! ") + ex.Message,
							"RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
			}
		}

		private void btnInstallNetworkLicenseServer_Click(object sender, EventArgs e)
		{
			var proc = new Process();
			string serverPath = Application.StartupPath;
			if (File.Exists("C:\\Program Files\\Hydronia\\LicenseManager\\CMSERVER.EXE"))
			{
				try
				{
					proc.StartInfo.Arguments = "/s";
					proc.StartInfo.FileName = "C:\\Program Files\\Hydronia\\LicenseManager\\CMSERVER.EXE";
					proc.StartInfo.UseShellExecute = true;
					proc.StartInfo.RedirectStandardOutput = false;
					proc.Start();
				}
				catch (Exception ex)
				{
					MessageBox.Show(Universal.Idioma("ERROR 100618.0713: error running RiverFlow2D network license server! ", "ERROR 100618.0659:¡error corriemdo el servidor de redes de RiverFlow2D¡ ") + ex.Message,
						"RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			else
			{
				MessageBox.Show(Universal.Idioma("RiverFlow2d network license server not found!", "¡No se encontró el servidor de licencias de redes de RiverFlow2D!"), "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

				var openFileDialog = new OpenFileDialog
				{
					Filter = "CMSERVER.EXE (*.exe)|*.exe",
					FilterIndex = 1,
					Title = "Choose CMSERVER.EXE",
					CheckFileExists = true,
					FileName = "CMSERVER.EXE"
				};

				if (openFileDialog.ShowDialog() == DialogResult.OK)
					try
					{
						proc.StartInfo.FileName = openFileDialog.FileName;
						proc.StartInfo.UseShellExecute = true;
						proc.StartInfo.RedirectStandardOutput = false;
						proc.Start();

					}
					catch (Exception ex)
					{
						MessageBox.Show(Universal.Idioma("ERROR 100618.0713: error running RiverFlow2D network license server! ", "ERROR 100618.0659:¡error corriemdo el servidor de redes de RiverFlow2D¡ ") + ex.Message,
							"RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void btnUpdates_Click(object sender, EventArgs e)
		{
			var wu = new WebUpdate();
			wu.RequestUpdate("after loading");
		}
	}
}
