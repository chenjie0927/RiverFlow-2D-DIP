using System.IO;

namespace Simulation_Options {
	class SedimentFile : ConfiguratedFile {


        public SedimentFile() {
            //Get the .sed configuration filename
					StreamReader s = File.OpenText(Universal.ConfigConfig);
            string read = s.ReadLine();
            while (read.Substring(0, 3) != "SED") read = s.ReadLine();
            s.Close();

            //Configurate the .SED filename
            CreateConfig(read.Substring(4));
        }

	}
}
