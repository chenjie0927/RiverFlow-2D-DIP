using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Deployment.Application;

namespace Simulation_Options
{
    partial class RiverFlo2DAboutBox : Form
    {
        public RiverFlo2DAboutBox()
        {
          InitializeComponent();
          Text = String.Format("About {0}", AssemblyTitle);
					labelProductName.Text = AssemblyProduct;
					labelVersion.Text = String.Format("Version {0}", AssemblyVersion);
          labelCopyright.Text = AssemblyCopyright;
          labelCompanyName.Text = AssemblyCompany;
          textBoxDescription.Text = AssemblyDescription;
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        private void RiverFlo2DAboutBox_Load(object sender, EventArgs e)
        {
            
	        if (RiverFlo2D.RiverDipModel == "Model CPU")
	        {
		        labelProductName.Text = "RiverFlow2D CPU";
		        labelVersion.Text = "Version 06.10.05";
		        labelCopyright.Text = "Copyright ©2009-2018";
                //textBoxDescription.Text = Universal.Idioma(
                //    "RiverFlow2D Plus is a hydrodynamic model for rivers and estuaries that uses a stable and powerful finite volume" +
                //    " method to compute high resolution flood hydraulics, including supercritical and subcritical flows over dry or wet river beds. " +
                //    "RiverFlow2D Plus offers several complementary components including one for Mud and Debris flow simulations. " +
                //    "RiverFlow2D Plus uses flexible triangular-cell meshes that allow resolving the flow field, pollutant concentration distribution, " +
                //    "sediment transport and bed elevation changes in complex river environments.", "");

                textBoxDescription.Text = Universal.Idioma(
                    "RiverFlow2D is a integrated hydrodynamic-hydrologic model for rivers, estuaries, and coastal areas,  that uses a stable and powerful finite volume method to compute high resolution flood hydraulics, including supercritical and subcritical flows over dry or wet river beds. RiverFlow2D offers several complementary components including hydraulic structures, Sediment Transport, Water Quality, and Mud and Debris flow simulations. RiverFlow2D uses flexible triangular-cell meshes that allow resolving the flow field, pollutant concentration distribution, sediment transport and bed elevation changes in any river, coastal, and floodplain environments.",

                    "RiverFlow2D es un modelo hidrodinámico-hidrológico para ríos, estuarios y zonas costeras que utiliza un método de volúmenes finitos para calcular la hidráulica de inundaciones en alta resolución, incluyendo flujos supercríticos y subcríticos sobre lechos de secos o mojados. RiverFlow2D ofrece varios componentes complementarios que incluyen estructuras hidráulicas,  simulaciones de transporte de sedimentos, calidad del agua y flujo de lodo y escombros. RiverFlow2D utiliza mallas flexibles de celdas triangulares que permiten resolver el campo de flujo, la distribución de la concentración de contaminantes, el transporte de sedimentos y los cambios de elevación del lecho en cualquier entorno fluvial, costero y llanuras de inundación.");                 
	        }
			else if (RiverFlo2D.RiverDipModel == "Model GPU")
			{
		        labelProductName.Text = "RiverFlow2D GPU";
		        labelVersion.Text = "Version 06.10.05";
		        labelCopyright.Text = "Copyright ©2009-2018";
				//textBoxDescription.Text = Universal.Idioma(
				//	"RiverFlow2D Plus GPU is a hydrodynamic model for rivers and estuaries that uses a stable and powerful " +
				//	"finite volume method to compute high resolution flood hydraulics, including supercritical and subcritical " +
				//	"flows over dry or wet river beds. It runs in specialized Graphical Processing Unit hardware and is able to " +
				//	"run orders of magnitude faster than RiverFlow2D Plus.  RiverFlow2D Plus GPU offers several complementary " +
				//	"components including one for Mud and Debris flow simulations. RiverFlow2D Plus GPU uses flexible " +
				//	"triangular-cell meshes that allow resolving the flow field, pollutant concentration distribution, " +
				//	"sediment transport and bed elevation changes in complex river environments.", "");

                textBoxDescription.Text = Universal.Idioma(
                    "RiverFlow2D is a integrated hydrodynamic-hydrologic model for rivers, estuaries, and coastal areas,  that uses a stable and powerful finite volume method to compute high resolution flood hydraulics, including supercritical and subcritical flows over dry or wet river beds. RiverFlow2D offers several complementary components including hydraulic structures, Sediment Transport, Water Quality, and Mud and Debris flow simulations. RiverFlow2D uses flexible triangular-cell meshes that allow resolving the flow field, pollutant concentration distribution, sediment transport and bed elevation changes in any river, coastal, and floodplain environments.",

                    "RiverFlow2D es un modelo hidrodinámico-hidrológico para ríos, estuarios y zonas costeras que utiliza un método de volúmenes finitos para calcular la hidráulica de inundaciones en alta resolución, incluyendo flujos supercríticos y subcríticos sobre lechos de secos o mojados. RiverFlow2D ofrece varios componentes complementarios que incluyen estructuras hidráulicas,  simulaciones de transporte de sedimentos, calidad del agua y flujo de lodo y escombros. RiverFlow2D utiliza mallas flexibles de celdas triangulares que permiten resolver el campo de flujo, la distribución de la concentración de contaminantes, el transporte de sedimentos y los cambios de elevación del lecho en cualquier entorno fluvial, costero y llanuras de inundación.");
            }
			else if (RiverFlo2D.RiverDipModel == "Model FE")
			{
				labelProductName.Text = "RiverFlow2D FE Model";
				labelVersion.Text = "Version 06.10.05";
				labelCopyright.Text = "Copyright ©2009-2018";
				textBoxDescription.Text = 
					"RiverFlow2D FE is a hydrodynamic  model for rivers and estuaries that uses a stable" +
					" and powerful finite element method to compute high resolution flood hydraulics, including " +
					"supercritical and subcritical flows over dry or wet river beds. RiverFlow2D FE offers Sediment " +
					"and Pollutant Transport Modules using a flexible triangular mesh that allow resolving the flow field, " +
					"pollutant concentration distribution, sediment transport and bed elevation changes in complex river environments.";
			}
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Close();
				}

		private void richTxtHydronia_LinkClicked(object sender, LinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(e.LinkText);
		}

		private void InstallUpdateSyncWithInfo()
		{
			UpdateCheckInfo info = null;

			if (ApplicationDeployment.IsNetworkDeployed)
			{
				ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;

				try
				{
					info = ad.CheckForDetailedUpdate();

				}
				catch (DeploymentDownloadException dde)
				{
					MessageBox.Show("The new version of the application cannot be downloaded at this time. \n\nPlease check your network connection, or try again later. Error: " + dde.Message);
					return;
				}
				catch (InvalidDeploymentException ide)
				{
					MessageBox.Show("Cannot check for a new version of the application. The ClickOnce deployment is corrupt. Please redeploy the application and try again. Error: " + ide.Message);
					return;
				}
				catch (InvalidOperationException ioe)
				{
					MessageBox.Show("This application cannot be updated. It is likely not a ClickOnce application. Error: " + ioe.Message);
					return;
				}

				if (info.UpdateAvailable)
				{
					Boolean doUpdate = true;

					if (!info.IsUpdateRequired)
					{
						DialogResult dr = MessageBox.Show("An update is available. Would you like to update the application now?", "Update Available", MessageBoxButtons.OKCancel);
						if (!(DialogResult.OK == dr))
						{
							doUpdate = false;
						}
					}
					else
					{
						// Display a message that the app MUST reboot. Display the minimum required version.
						MessageBox.Show("This application has detected a mandatory update from your current " +
							"version to version " + info.MinimumRequiredVersion.ToString() +
							". The application will now install the update and restart.",
							"Update Available", MessageBoxButtons.OK,
							MessageBoxIcon.Information);
					}

					if (doUpdate)
					{
						try
						{
							ad.Update();
							MessageBox.Show("The application has been upgraded, and will now restart.");
							Application.Restart();
						}
						catch (DeploymentDownloadException dde)
						{
							MessageBox.Show("Cannot install the latest version of the application. \n\nPlease check your network connection, or try again later. Error: " + dde);
							return;
						}
					}
				}
			}
		}

		private void richTxtHydronia_TextChanged(object sender, EventArgs e)
		{

		}
	}
}
