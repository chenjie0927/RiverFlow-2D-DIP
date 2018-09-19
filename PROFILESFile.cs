using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Simulation_Options
{
    class PROFILESFile : InputFile
    {

        public PROFILESFile()
        {

            var emptyPROFILES = new List<string[]>();
            AddVariable("PROFILE_VALUES", emptyPROFILES);
            AddVariable("NUMBER_OF_PROFILES", 0);
        }

        //Load data from file
        public virtual void Load(string filename)
        {
            //Load all data into the data manager
            StreamReader s = File.OpenText(filename);

            try
            {
	            string read = s.ReadLine();
                int numberOfPROFILES = Convert.ToInt32(read);

                AddVariable("NUMBER_OF_PROFILES", numberOfPROFILES);

                var profiles = new List<string[]>();
                for (int i = 0; i < numberOfPROFILES; ++i)
                {
	                //profile name
                    string name = s.ReadLine().Trim();

                    string line = s.ReadLine();
                    string[] split = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    var row = new string[3];

                    row[0] = name;
                    // number of vertices:
                    row[1] = split[0];
                    // number of intervals:
                    row[2] = split[1];

					for (int j = 0; j < Convert.ToInt32(row[1]); ++j)
					{
                        var row2 = new string[5];

                        //Name
                        row2[0] = row[0];
                        //N vertices
                        row2[1] = row[1];
                        //N intervals
                        row2[2] = row[2];
                        //x1 y1
                        line = s.ReadLine();
                        split = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        row2[3] = split[0];
                        row2[4] = split[1];

                        profiles.Add(row2);
				    }

                }

                AddVariable("PROFILE_VALUES", profiles);

            }
            catch 
            {
                MessageBox.Show(Universal.Idioma("ERROR 2199110811: error trying to read .PROFILES file.", "ERROR 2199110811: error leyendo archivo .PROFILES."),  
                    "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            s.Close();
        }

        //Save data to file
        public virtual void Save(string filename)
        {
            TextWriter w = new StreamWriter(filename);
            try
            {
                var profiles = (List<string[]>)GetVariable("PROFILE_VALUES");
                var n_profiles = (int)GetVariable("NUMBER_OF_PROFILES");

                //number of profiles 
                w.WriteLine(n_profiles);

                string name = "";
                for (int j = 0; j < profiles.Count; ++j)
                {
                    if (name != profiles[j][0].Trim())
                    {
                        //profiled name
                        w.WriteLine(profiles[j][0]);
                        name = profiles[j][0].Trim();
                        //Number of vertices & profile coeff
                        w.WriteLine(profiles[j][1] + " " + profiles[j][2]);
                    }

                    //X, Y
                    w.WriteLine(profiles[j][3] + " " + profiles[j][4]);
                }



             //   var profiles = (List<string[]>)GetVariable("PROFILE_VALUES");
             //   var numberOfRows = (int)GetVariable("NUMBER_OF_PROFILES");

	            ////number of profiles 
             //   w.WriteLine(profiles.Count);

             //   string name = "";
             //   for (int j = 0; j < profiles.Count; ++j)
             //   {
             //       if (name != profiles[j][0].Trim())
             //       {
             //           //profiled name
             //           w.WriteLine(profiles[j][0]);
             //           name = profiles[j][0].Trim();
             //           //Number of vertices & profile coeff
             //           w.WriteLine(profiles[j][1] + " " + profiles[j][2]);
             //       }

             //       //X, Y
             //       w.WriteLine(profiles[j][3] + " " + profiles[j][4]);
             //   }

            }
            catch 
            {
                MessageBox.Show(Universal.Idioma("ERROR 2199110812: error trying to save .PROFILES file.", "ERROR 2199110812: error leyendo archivo .PROFILES."), 
                    "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            w.Close();
        }
    }
}

