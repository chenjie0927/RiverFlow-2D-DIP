using System.IO;

namespace Simulation_Options {
	class PLOTFile : ConfiguratedFile{

        public PLOTFile() {
            //Get the .PLOT configuration filename
            StreamReader s = File.OpenText(Universal.ConfigConfig);
            string read = s.ReadLine();
            while (read.Substring(0, 4) != "PLOT") read = s.ReadLine();
            s.Close();

            //Configurate the .PLOT filename
            CreateConfig(read.Substring(5));
        }


    }
}