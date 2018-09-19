using System.IO;

namespace Simulation_Options
{
	class WINDFile : ConfiguratedFile
	{

		public WINDFile()
		{
			//Get the .WIND configuration filename
			StreamReader s = File.OpenText(Universal.ConfigConfig);
			string read = s.ReadLine();
			while (read.Substring(0, 4) != "WIND") read = s.ReadLine();
			s.Close();

			//Configurate the .WIND filename
			CreateConfig(read.Substring(5));
		}

	}
}