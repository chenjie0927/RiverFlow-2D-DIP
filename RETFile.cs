using System.IO;

namespace Simulation_Options {
	class RETFile : ConfiguratedFile{

        public RETFile()
        {
            //Get the .ret configuration filename
					StreamReader s = File.OpenText(Universal.ConfigConfig);
            string read = s.ReadLine();
            while (read.Substring(0, 3) != "RET") read = s.ReadLine();
            s.Close();

            //Configurate the .ret filename
            CreateConfig(read.Substring(4));
        }
	}
}
