using System.IO;

namespace Simulation_Options {
	class PLTFile : ConfiguratedFile{

        public PLTFile() {
            //Get the .PLT configuration filename
					StreamReader s = File.OpenText(Universal.ConfigConfig);
            string read = s.ReadLine();
            while (read.Substring(0, 3) != "PLT") read = s.ReadLine();
            s.Close();

            //Configurate the .PLT filename
            CreateConfig(read.Substring(4));
        }

    }
}
