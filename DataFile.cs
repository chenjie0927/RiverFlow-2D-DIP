using System.IO;

namespace Simulation_Options {
	class DataFile : ConfiguratedFile {

        public DataFile()
        {
            //Get the .dat configuration filename
			StreamReader s = File.OpenText(Universal.ConfigConfig);
            string read = s.ReadLine();
            while (read.Substring(0, 3) != "DAT") read = s.ReadLine();
            s.Close();

            //Configurate the .dat filename
            CreateConfig(read.Substring(4));

          //  dataManager.set("IINITIAL0", (int)dataManager.get("IINITIAL0") + 1);
        }

	}
}
