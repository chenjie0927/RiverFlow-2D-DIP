using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows.Forms;

namespace Simulation_Options
{

	// helper class
	class OutputClass
	{
		string myString;

		// Constructor
		public OutputClass(string inputString)
		{
			myString = inputString;
		}

		// Instance Method
		public void printString()
		{
			Console.WriteLine("{0}", myString);
		}

		// Destructor
		~OutputClass()
		{
			// Some resource cleanup routines
		}
	}



	class WebUpdate
	{
		public WebUpdate()
		{
		
		}

		public void RequestUpdate(string loading)
		{
			try
			{
				Cursor.Current = Cursors.WaitCursor;

				// Get version number from server:
				WebRequest request = WebRequest.Create("http://www.riverflow2d.com/current_version.txt");
				request.Credentials = CredentialCache.DefaultCredentials;
				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
				Stream datastream = response.GetResponseStream();
				StreamReader reader = new StreamReader(datastream);
				string serverVersion = reader.ReadToEnd();
				reader.Close();
				response.Close();

				// Convert serverVersion to pairs of digits separated by dots.
				string[] split = new string[serverVersion.Length / 2 + (serverVersion.Length % 2 == 0 ? 0 : 1)];
				for (int i = 0; i < split.Length; i++)
				{
					split[i] = serverVersion.Substring(i * 2, i * 2 + 2 > serverVersion.Length ? 1 : 2);
				}
				string dotyServerVersion = split[1] + "." + split[2] + "." + split[3]; 

				// Get version number from local file:
				StreamReader s = File.OpenText("current_version.txt");
				string localVersion = s.ReadLine();
				string show = s.ReadLine();
				s.Close();

				// Convert localVersion to pairs of digits separated by dots.
				split = new string[localVersion.Length / 2 + (localVersion.Length % 2 == 0 ? 0 : 1)];
				for (int i = 0; i < split.Length; i++)
				{
					split[i] = localVersion.Substring(i * 2, i * 2 + 2 > localVersion.Length ? 1 : 2);
				}
				string dotyLocalVersion = split[1] + "." + split[2] + "." + split[3];

				int local = Convert.ToInt32(localVersion);
				int server =Convert.ToInt32(serverVersion.Substring(0,8));
				if (local == server)
				{
					if (loading == "after loading")
					MessageBox.Show(Universal.Idioma("RiverFlow2D is up to date!","¡RiverFlow2D está actualizado!\n\nVersión actual: " + dotyLocalVersion),
													"RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				if (local > server)
				{
					MessageBox.Show(
						Universal.Idioma("Your installed RiverFlow2D version seems to be wrong!.\n\nPlease contact Hydronia.\n\nYour Version: ",  
						"¡La version instalada de RiverFlow2D parece equivocada!\n\nPor favor contacte Hydronia.\n\nSu Versión: ") + 
					     dotyLocalVersion + Universal.Idioma("\nCurrent Version: " ,"\nVersión mas reciente: ") + dotyServerVersion,  
						"RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					if (loading == "loading")
					{
						if (MessageBox.Show(Universal.Idioma("Would you like to see this message again?", "¿Desea seguir viendo este mensaje?"),
							"RiverFlow2D", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
						{
							TextWriter w = new StreamWriter("current_version.txt");
							w.WriteLine(localVersion);
							w.WriteLine("0");
							w.Close();
						}
					}
					else
					{
						TextWriter w = new StreamWriter("current_version.txt");
						w.WriteLine(localVersion);
						w.WriteLine("1");
						w.Close();
					}
				}
				else if (local < server )
				{
					DialogResult dlgResult =
						MessageBox.Show(Universal.Idioma("There is a new version of RiverFlow2D. Would you like to update it?.\n\nYour Version: ",
														 "Hay una nueva version of RiverFlow2D. ¿Desea actualizarla?\n\nSu Versión: ") + 
														 dotyLocalVersion + Universal.Idioma("\nCurrent Version: ", "\nVersión mas reciente: ") + dotyServerVersion,
														 "RiverFlow2D", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

					if (dlgResult == DialogResult.Yes)
						System.Diagnostics.Process.Start("http://www.hydronia.com/software-updates/");

					if (loading == "loading")
					{
						if (MessageBox.Show(Universal.Idioma("Would you like to see this message again?", "¿Desea seguir viendo este mensaje?"),
							"RiverFlow2D", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
						{
							TextWriter w = new StreamWriter("current_version.txt");
							w.WriteLine(localVersion);
							w.WriteLine("0");
							w.Close();
						}
					}
					else
					{
						TextWriter w = new StreamWriter("current_version.txt");
						w.WriteLine(localVersion);
						w.WriteLine("1");
						w.Close();
					}
			}
			}
			catch (Exception ex)
			{
				if (loading == "after loading")
					MessageBox.Show(Universal.Idioma("ERROR 160718_1626: error looking for update of RiverFlow2D.", "ERROR 160718_1626: error buscando actualización de RiverFlow2D.") +
					Environment.NewLine + ex.Message, "RiverFlow2D",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

	}
}
