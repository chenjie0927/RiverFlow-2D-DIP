using System.IO;

namespace Simulation_Options
{
    class PIERSFile : ConfiguratedFile
    {

        public PIERSFile()
        {
            //Get the .ret configuration filename
					StreamReader s = File.OpenText(Universal.ConfigConfig);
            string read = s.ReadLine();
            while (read.Substring(0, 5) != "PIERS") read = s.ReadLine();
            s.Close();

            //Configure the .PIERS filename
            CreateConfig(read.Substring(6));
        }
    }
}