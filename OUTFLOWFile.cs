using System.IO;

namespace Simulation_Options
{
    class OUTFLOWFile : ConfiguratedFile
    {
        public OUTFLOWFile()
        {
            //Get the .ret configuration filename
					StreamReader s = File.OpenText(Universal.ConfigConfig);
            string read = s.ReadLine();
            while (read.Substring(0, 7) != "OUTFLOW") read = s.ReadLine();
            s.Close();
            //Configurate the .ret filename
            CreateConfig(read.Substring(9));
        }
    }
}