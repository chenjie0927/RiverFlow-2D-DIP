using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Simulation_Options
{
	static class Help
	{
		static Dictionary<string, int > TabPages =  new Dictionary<string, int>() ;

		public static void OpenHelp()
		{
			//Show help
			try
			{
				string filePathAndName = Application.StartupPath + "\\RiverFlow2D_Reference_Manual.pdf";
				bool fileExists = File.Exists(filePathAndName);
				if (!fileExists)
				{
					if (DialogResult.OK ==
							MessageBox.Show(Universal.Idioma(
                                "RiverFlow2D_Reference_Manual.pdf file could not be found.\nPlease, search for a directory to find it.",
                                "El archivo RiverFlow2D_Reference_Manual.pdf no se pudo encontrar.\nPor favor, indique un directorio para localizarlo."),
								"RiverFlow2D", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation))
					{

						var openFileDialog = new OpenFileDialog
						{
							Filter = "RiverFlow2D help (*.pdf)|*.pdf",
							FileName = "RiverFlow2D_Reference_Manual.pdf",
							FilterIndex = 1,
							RestoreDirectory = true,
							CheckFileExists = true,
							ShowHelp = true
						};

						if (openFileDialog.ShowDialog() == DialogResult.OK)
						{
							filePathAndName = openFileDialog.FileName;
							fileExists = true;
						}
					}
				}
				if (fileExists)
				{
					string page = "1";
					var pdfProcess = new Process();
					pdfProcess.StartInfo.FileName = "AcroRd32.exe";
					pdfProcess.StartInfo.Arguments = "/A \"page=" + page + "\"C:\\TRACKS\\DOCS\\RiverFlow2D\\RiverFlow2D Manuals\\RiverFlow2D_Reference_Manual.pdf";
					pdfProcess.Start();
					//Process.Start(filePathAndName,"/a Page=5");

				}

			}
			catch (Exception ex)
			{
				MessageBox.Show(Universal.Idioma("Could not process the help file. ", "No se pudo procesar el archivo de ayuda")
                    + ex.Message, "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private static void LoadHelpPages()
		{
			//Get help pages for each tab
			StreamReader s = File.OpenText("help.config");

			while (!s.EndOfStream)
			{
				string read = s.ReadLine();
				string[] split = read.Split(new[] { ' ', '\t' });
				TabPages.Add(split[0], Convert.ToInt32(split[1]));
			}
			s.Close();
		}
	}

	




}
