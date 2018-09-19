using System;
using System.IO;
using System.Windows.Forms;

namespace Simulation_Options
{
	public partial class ModelSelection : Form
	{
		public ModelSelection()
		{
			InitializeComponent();
		}

		private void ModelSelection_Load(object sender, EventArgs e)
		{
			if (RiverFlo2D.RiverDipModel=="Model GPU")
				radioPlusGPU.Checked = true;
			else if (RiverFlo2D.RiverDipModel=="Model CPU")
				radioPlus.Checked = true;
			else if (RiverFlo2D.RiverDipModel=="Model 4") 
				radioV4.Checked = true;
			else
				radioV4.Checked = true;
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
            SelectModel();
		}

        private void SelectModel()
        {
            string newModel = "";
            if (radioPlus.Checked && RiverFlo2D.RiverDipModel != "Model CPU")
            {
                newModel = "Model CPU";
            }
            else if (radioPlusGPU.Checked && RiverFlo2D.RiverDipModel != "Model GPU")
            {
                newModel = "Model GPU";
            }

            if (newModel != "")
            {
                //Change model name in version.config.
                TextWriter sr = new StreamWriter("version.config");
                sr.WriteLine(newModel);
                sr.Close();
                RiverFlo2D.RiverDipNewModel = newModel;
            }
            Close();
        }

		private void btnCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

        private void groupRiverFLOmodel_Enter(object sender, EventArgs e)
        {

        }
    }
}
