using System.IO;

namespace Simulation_Options
{
	class ADFile : ConfiguratedFile
	{

		public ADFile()
		{
			//Get the .AD configuration filename
			StreamReader s = File.OpenText(Universal.ConfigConfig);
			string read = s.ReadLine();
			while (read.Substring(0, 2) != "AD") read = s.ReadLine();
			s.Close();

			//Configurate the .AD filename
			CreateConfig(read.Substring(3));
		}


	}
}
