using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Simulation_Options
{
    class WEIRSFile : InputFile
    {
			public WEIRSFile()
        {
            var emptyWeirs = new List<string[]>();
            AddVariable("WEIR_VALUES", emptyWeirs);
            AddVariable("NUMBER_OF_WEIRS", 0);
        }

        //Load data from file
        public virtual void Load(string filename)
        { 
            //Load all data into the data manager
            StreamReader s = File.OpenText(filename);

            try
            {
	            string read = s.ReadLine();
                int numberOfWEIRS = Convert.ToInt32(read);

                AddVariable("NUMBER_OF_WEIRS", numberOfWEIRS);
               
                var weirs = new List<string[]>();
                for (int i = 0; i < numberOfWEIRS; ++i)
                {
                    //string[] row = new string[2];

	                //weir name
                    string name = s.ReadLine();
                    string line = s.ReadLine();
                    string[] split = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    var row = new string[4];

                    row[0] = name.Trim();
                    // number of vertices
                    row[1] = split[0];
                    // weir coefficient
                    row[2] = split[1];
										// crest elevation
										row[3] = split[2];

                    for (int j = 0; j < Convert.ToInt32(row[1]); ++j)
                    {
                        var row2 = new string[6];

                        //Name
                        row2[0] = row[0];
                        //N vertices
                        row2[1] = row[1];
                        //weir coefficient
                        row2[2] = row[2];
												//crest elevation
												//row2[3] = row[3];
                        //x1 y1
                        line = s.ReadLine();
                        split = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        row2[4] = split[0];  //X
                        row2[5] = split[1];  //Y
                        row2[3] = split[2];  // Crest Elevation

                        weirs.Add(row2);
                    }

                }

                AddVariable("WEIR_VALUES", weirs);

            }
            catch 
            {
                MessageBox.Show(Universal.Idioma("ERROR 1199112353: error trying to read .WEIRS file.", "ERROR 1199112353: error leyendo archivo .WEIRS."), 
                    "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error) ;
            }

            s.Close();

        }

        //Save data to file
        public virtual void Save(string filename) 
        {
            TextWriter w = new StreamWriter(filename);
            try
            {

                var weirs = (List<string[]>)GetVariable("WEIR_VALUES");
                int numberOfWEIRS = (int)GetVariable("NUMBER_OF_WEIRS");

                if (numberOfWEIRS > 0)
                {
	                //number of weirds 
                    w.WriteLine(numberOfWEIRS);

                    string name = "";
                    for (int j = 0; j < weirs.Count; ++j)
                    {
                        if (name != weirs[j][0].Trim())
                        {
                            //weird name
                            w.WriteLine(weirs[j][0]);
                            name = weirs[j][0].Trim();
                            //Number of vertices & weir coeff & crest elevation
														w.WriteLine(weirs[j][1] + " " + weirs[j][2] + " " + weirs[j][3]);
                        }

                        //X, Y
                        w.WriteLine(weirs[j][4] + " " + weirs[j][5] + " " + weirs[j][3]);
                    }
                }
                else
                {
                    //saveAllwarnings += "{0} " + "The weirs table is empty. It was not saved.";
                   MessageBox.Show(Universal.Idioma("The weirs table is empty. It was not saved.", "La tabla de vertederos está vacía. No fue almacenada."),
                       "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }                      
            }
            catch 
            {
                MessageBox.Show(Universal.Idioma("ERROR 1199119251: error trying to save .WEIRS file.", "ERROR 1199119251: error almacenando archivo .WEIRS."),
                    "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            w.Close();
        }
    }
}

