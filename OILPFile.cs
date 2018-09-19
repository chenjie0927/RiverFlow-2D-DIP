using System.IO;

namespace Simulation_Options
{
	class OILPFile : ConfiguratedFile
	{

		public OILPFile()
		{
			//Get the .OILP configuration filename
			StreamReader s = File.OpenText(Universal.ConfigConfig);
			string read = s.ReadLine();
			while (read.Substring(0, 4) != "OILP") read = s.ReadLine();
			s.Close();

			//Configurate the .OILP filename
			CreateConfig(read.Substring(5));
		}

	}
}
