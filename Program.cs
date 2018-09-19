using System;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace Simulation_Options {
	static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
        /// 

		[STAThread]
    static void Main(string[] args) 
		{

			try
			{

				Application.EnableVisualStyles();

				Application.SetCompatibleTextRenderingDefault(false);

     			string[] arguments = Environment.GetCommandLineArgs();

				//Check that .2DM exists:
				if (args.Length > 0)
				{
					var file2DM = args[0];

					Boolean exists = File.Exists(file2DM);
					if (exists)
					{
						//MessageBox.Show("% " + "(Main) " + ".2DM file: " + file2DM);
						string fileDAT = file2DM.Remove(file2DM.Length - 3) + "DAT";
						if (File.Exists(fileDAT))
						{
							//MessageBox.Show("% " + "(Main) " + ".DAT file: " + file2DM);
							RiverFlo2D._DATPathAndFile = fileDAT;
						}
						else
							MessageBox.Show(Universal.Idioma("ERROR 3003160939: file " + fileDAT + " doesn´t exist.", "ERROR 3003160939: archivo " + fileDAT + " no existe."),
								"RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

					}
					else
						MessageBox.Show(Universal.Idioma("ERROR 1203160950: file " + file2DM + " doesn´t exist.", "ERROR 1203160950: archivo " + file2DM + " no existe."),
								"RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}

                RiverFlo2D river = new RiverFlo2D();
                Application.Run(river);
			}
			catch (Exception ex)
			{
				MessageBox.Show(Universal.Idioma("ERROR 0403160714: error while starting Main in Program Class. ", "ERROR 0403160714: error comenzando Main en Program Class. ") + 
                    ex.Message, "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}