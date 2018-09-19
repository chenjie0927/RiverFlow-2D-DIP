using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Simulation_Options
{
	public partial class NewProject : Form
	{
		public NewProject()
		{
			InitializeComponent();
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			if (txtProjectNameAndLocation.Text == "" )
				MessageBox.Show(Universal.Idioma("A project name is required.", "Se requiere un nombre de proyecto."), 
                    "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			else
			{
				if (radioPlusGPU.Checked)
				{
					Universal.OpenProjectExit = "Model GPU";
					Close();
				}
				else if (radioPlus.Checked)
				{
					Universal.OpenProjectExit = "Model CPU";
					Close();
				}
				else if (radioFE.Checked)
				{
					Universal.OpenProjectExit = "Model FE";
					Close();
				}
				Universal.ProjectPathAndName = txtProjectNameAndLocation.Text;
			}
		}

		private void NewProject_Load(object sender, EventArgs e)
		{
			radioPlus.Checked = true;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			Universal.OpenProjectExit = "Cancel";
			Close();
		}

		private void btnProjectNameAndLocation_Click(object sender, EventArgs e)
		{
			var saveFileDialog = new SaveFileDialog
				{
					Filter = "DAT files (*.DAT)|*.DAT",
					FileName = "",
					FilterIndex = 1,
					RestoreDirectory = false,
					OverwritePrompt = false
				};

			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				if (saveFileDialog.FileName == "")
					MessageBox.Show(Universal.Idioma("A project name is required.", "Se require un nombre de proyecto"), 
                        "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				else
					txtProjectNameAndLocation.Text = saveFileDialog.FileName;
			}
		}
	}
}
