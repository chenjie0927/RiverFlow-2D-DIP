using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace Simulation_Options
{
	public partial class OpenProject : Form 
	{
		public OpenProject()
		{
			InitializeComponent();
		}

		private void OpenProject_Load(object sender, EventArgs e)
		{
			Icon = Properties.Resources.RiverFLO_2D_Icon_4;
			Universal.OpenProjectExit = "Cancel";
			ToolTip tt = new ToolTip();
			lblProject0.Text = "";
			lblProject1.Text = "";
			lblProject2.Text = "";
			lblProject3.Text = "";
			lblProject4.Text = "";
			
			string recentProjectsFile =  Path.GetTempPath() + "RecentProjects.TXT";
			for (int i = 0; i <= 4; i++)
				if (Universal.RecentProjects[i] != null)
					if (Universal.RecentProjects[i].Trim() != "")
						switch (i)
						{
							case 0:
								lblProject0.Text = Universal.RecentProjects[i];
								tt.SetToolTip(lblProject0, lblProject0.Text);
								break;

							case 1:
								lblProject1.Text = Universal.RecentProjects[i];
								tt.SetToolTip(lblProject1, lblProject1.Text);
								break;

							case 2:
								lblProject2.Text = Universal.RecentProjects[i];
								tt.SetToolTip(lblProject2, lblProject2.Text);
								break;

							case 3:
								lblProject3.Text = Universal.RecentProjects[i];
								tt.SetToolTip(lblProject3, lblProject3.Text);
								break;

							case 4:
								lblProject4.Text = Universal.RecentProjects[i];
								tt.SetToolTip(lblProject4, lblProject4.Text);
								break;
						}
		}



		private void btnOpenExisting_Click(object sender, EventArgs e)
		{
            Universal.OpenProjectExit = "OpenProject";
			Close();
		}

		private void btnNew_Click(object sender, EventArgs e)
		{
			var newProject = new NewProject();
			newProject.ShowDialog();
			if (Universal.OpenProjectExit != "Cancel")
				Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			Universal.OpenProjectExit = "Cancel";
			Close();
		}


		private void lblProject0_Click(object sender, EventArgs e)
		{
			if (lblProject0.Text != "")
			{
				Universal.OpenProjectExit = lblProject0.Text;
                Close();
			}
		}

		private void lblProject0_DoubleClick(object sender, EventArgs e)
		{

		}

		private void lblProject1_DoubleClick(object sender, EventArgs e)
		{
			Universal.OpenProjectExit = lblProject1.Text;
			Close();
		}

		private void lblProject2_DoubleClick(object sender, EventArgs e)
		{
			Universal.OpenProjectExit = lblProject2.Text;
			Close();
		}

		private void lblProject3_DoubleClick(object sender, EventArgs e)
		{
			Universal.OpenProjectExit = lblProject3.Text;
			Close();
		}

		private void lblProject4_DoubleClick(object sender, EventArgs e)
		{
			Universal.OpenProjectExit = lblProject4.Text;
			Close();
		}

		private void lblProject0_MouseEnter(object sender, EventArgs e)
		{
			//var lbl = (Label) sender;
			//lbl.BorderStyle = BorderStyle.Fixed3D;
			//lbl.BackColor = Color.DarkGray;
			lblProject0.BorderStyle = BorderStyle.Fixed3D;
			lblProject0.BackColor = SystemColors.GradientActiveCaption;
		}

		private void lblProject0_MouseLeave(object sender, EventArgs e)
		{
			//var lbl = (Label) sender;
			//lbl.BorderStyle = BorderStyle.None;
			//lbl.BackColor = SystemColors.Control;
			lblProject0.BorderStyle = BorderStyle.None;
			lblProject0.BackColor = SystemColors.GradientInactiveCaption;
		}

		private void lblProject1_MouseEnter(object sender, EventArgs e)
		{
			//var lbl = (Label) sender;
			//lbl.BorderStyle = BorderStyle.Fixed3D;
			//lbl.BackColor = Color.DarkGray;
			lblProject1.BorderStyle = BorderStyle.Fixed3D;
			lblProject1.BackColor = SystemColors.GradientActiveCaption;
		}

		private void lblProject2_MouseEnter(object sender, EventArgs e)
		{
			//var lbl = (Label) sender;
			//lbl.BorderStyle = BorderStyle.Fixed3D;
			//lbl.BackColor = Color.DarkGray;
			lblProject2.BorderStyle = BorderStyle.Fixed3D;
			lblProject2.BackColor = SystemColors.GradientActiveCaption;
		}

		private void lblProject3_MouseEnter(object sender, EventArgs e)
		{
			//var lbl = (Label) sender;
			//lbl.BorderStyle = BorderStyle.Fixed3D;
			//lbl.BackColor = Color.DarkGray;
			lblProject3.BorderStyle = BorderStyle.Fixed3D;
			lblProject3.BackColor = SystemColors.GradientActiveCaption;
		}

		private void lblProject4_MouseEnter(object sender, EventArgs e)
		{
			//var lbl = (Label) sender;
			//lbl.BorderStyle = BorderStyle.Fixed3D;
			//lbl.BackColor = Color.DarkGray;
			lblProject4.BorderStyle = BorderStyle.Fixed3D;
			lblProject4.BackColor = SystemColors.GradientActiveCaption;
		}

		private void lblProject1_MouseLeave(object sender, EventArgs e)
		{
			//var lbl = (Label) sender;
			//lbl.BorderStyle = BorderStyle.None;
			//lbl.BackColor = SystemColors.Control;
			lblProject1.BorderStyle = BorderStyle.None;
			lblProject1.BackColor = SystemColors.GradientInactiveCaption;
		}

		private void lblProject2_MouseLeave(object sender, EventArgs e)
		{
			//var lbl = (Label) sender;
			//lbl.BorderStyle = BorderStyle.None;
			//lbl.BackColor = SystemColors.Control;
			lblProject2.BorderStyle = BorderStyle.None;
			lblProject2.BackColor = SystemColors.GradientInactiveCaption;
		}

		private void lblProject3_MouseLeave(object sender, EventArgs e)
		{
			//var lbl = (Label) sender;
			//lbl.BorderStyle = BorderStyle.None;
			//lbl.BackColor = SystemColors.Control;
			lblProject3.BorderStyle = BorderStyle.None;
			lblProject3.BackColor = SystemColors.GradientInactiveCaption;
		}

		private void lblProject4_MouseLeave(object sender, EventArgs e)
		{
			//var lbl = (Label) sender;
			//lbl.BorderStyle = BorderStyle.None;
			//lbl.BackColor = SystemColors.Control;
			lblProject4.BorderStyle = BorderStyle.None;
			lblProject4.BackColor = SystemColors.GradientInactiveCaption;
		}

		private void OpenProject_MouseEnter(object sender, EventArgs e)
		{
			lblProject0.BorderStyle = BorderStyle.None;
			lblProject0.BackColor = SystemColors.GradientInactiveCaption;
			lblProject1.BorderStyle = BorderStyle.None;
			lblProject1.BackColor = SystemColors.GradientInactiveCaption;
			lblProject2.BorderStyle = BorderStyle.None;
			lblProject2.BackColor = SystemColors.GradientInactiveCaption;
			lblProject3.BorderStyle = BorderStyle.None;
			lblProject3.BackColor = SystemColors.GradientInactiveCaption;
			lblProject4.BorderStyle = BorderStyle.None;
			lblProject4.BackColor = SystemColors.GradientInactiveCaption;
		}

		private void lblProject1_MouseHover(object sender, EventArgs e)
		{
			lblProject1.BorderStyle = BorderStyle.Fixed3D;
			lblProject1.BackColor = SystemColors.GradientActiveCaption;
		}

		private void lblProject0_MouseHover(object sender, EventArgs e)
		{
			lblProject0.BorderStyle = BorderStyle.Fixed3D;
			lblProject0.BackColor = SystemColors.GradientActiveCaption;
		}

		private void lblProject2_MouseHover(object sender, EventArgs e)
		{
			lblProject2.BorderStyle = BorderStyle.Fixed3D;
			lblProject2.BackColor = SystemColors.GradientActiveCaption;
		}

		private void lblProject3_MouseHover(object sender, EventArgs e)
		{
			lblProject3.BorderStyle = BorderStyle.Fixed3D;
			lblProject3.BackColor = SystemColors.GradientActiveCaption;
		}

		private void lblProject4_MouseHover(object sender, EventArgs e)
		{
			lblProject4.BorderStyle = BorderStyle.Fixed3D;
			lblProject4.BackColor = SystemColors.GradientActiveCaption;
		}

		private void lblProject1_Click(object sender, EventArgs e)
		{
			if (lblProject1.Text != "")
			{
				Universal.OpenProjectExit = lblProject1.Text;
				Close();
			}
		}

		private void lblProject2_Click(object sender, EventArgs e)
		{
			if (lblProject2.Text != "")
			{
				Universal.OpenProjectExit = lblProject2.Text;
				Close();
			}
		}

		private void lblProject3_Click(object sender, EventArgs e)
		{
			if (lblProject3.Text != "")
			{
				Universal.OpenProjectExit = lblProject3.Text;
				Close();
			}
		}

		private void lblProject4_Click(object sender, EventArgs e)
		{
			if (lblProject4.Text != "")
			{
				Universal.OpenProjectExit = lblProject4.Text;
				Close();
			}
		}

	}
}
