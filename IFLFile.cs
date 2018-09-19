using System;
using System.IO;
using System.Windows.Forms;

namespace Simulation_Options
{
    class IFLFile : ConfiguratedFile
    {
        public IFLFile()
        {
            //Get the .IFL configuration filename
            StreamReader s = File.OpenText(Universal.ConfigConfig);
            string read = s.ReadLine();
            while (read.Substring(0, 3) != "IFL") read = s.ReadLine();
            s.Close();

            //Configure the .IFL filename
            CreateConfig(read.Substring(4));
        }

        //read file names
        public string[] ReadFileNames(string filename)
        {
            StreamReader s = File.OpenText(filename);
            string read = s.ReadLine();
            int numberOfNodes = Convert.ToInt32(read);

            var fileNames = new string[numberOfNodes];

            try
            {
	            for (int i = 0; i < numberOfNodes; ++i)
                {
                    //node, boundary condition type, file name      
                    string line = s.ReadLine();
                    string[] split = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    fileNames[i] = split[2];
                }

            }
            catch
            {
                MessageBox.Show(Universal.Idioma("ERROR 1412111009: error trying to read .IFL file.", "ERROR 1412111009: error leyendo archivo .IFL."), 
                    "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            s.Close();

            return fileNames;

        }

    }
}

