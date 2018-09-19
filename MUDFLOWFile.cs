using System.IO;

namespace Simulation_Options
{
	class MUDFLOWFile : ConfiguratedFile
	{

		public MUDFLOWFile()
		{
			//Get the .MUD configuration filename
			StreamReader s = File.OpenText(Universal.ConfigConfig);
			string read = s.ReadLine();
			while (read.Substring(0, 3) != "MUD") read = s.ReadLine();
			s.Close();

			//Configurate the .MUD filename
			CreateConfig(read.Substring(4));
		}

	}
}
