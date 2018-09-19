using System.IO;

namespace Simulation_Options
{
    class INFLOWFile : ConfiguratedFile
    {
       
        public INFLOWFile() {
            //Get the .sed configuration filename
					StreamReader s = File.OpenText(Universal.ConfigConfig);
            string read = s.ReadLine();
            while (read.Substring(0, 6) != "INFLOW") read = s.ReadLine();
            s.Close();

            //Configurate the .INF filename
            CreateConfig(read.Substring(7));
        }
    }
}
