using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Simulation_Options
{
    class OBSFile : InputFile
    {

        public OBSFile()
        {

            var emptyObs = new List<string[]>();
            AddVariable("OBS_VALUES", emptyObs);
            AddVariable("NUMBER_OF_OBS", 0);
        }

        //Load data from file
        public virtual void Load(string filename)
        {
            //Load all data into the data manager
            StreamReader s = File.OpenText(filename);

            try
            {

                string read = s.ReadLine();
                int numberOfOBS = Convert.ToInt32(read);

                AddVariable("NUMBER_OF_OBS", numberOfOBS);

                var obs = new List<string[]>();

                for (int i = 0; i < numberOfOBS; ++i)
                {
                    //string[] row = new string[2];

	                //obs name
                    string name = s.ReadLine();

                    var row = new string[3];

                    row[0] = name.Trim();
                    string line = s.ReadLine();

                    //x1 y1
                    string[] split = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    row[1] = split[0];
                    row[2] = split[1];

                    obs.Add(row);
                    
                }

                AddVariable("OBS_VALUES", obs);

            }
            catch 
            {
                MessageBox.Show(Universal.Idioma("ERROR 1599110742: error trying to read .OBS file.", "ERROR 1599110742: error leyendo archivo .OBS."),
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
                var obs = (List<string[]>)GetVariable("OBS_VALUES");

	            var variable = (int) GetVariable("NUMBER_OF_OBS");

	            new List<string>();

								if (false)
                { // This only if more than one point per name:
                    //int k = 0;

                    //for (int i = 0; i < number_of_obs; ++i)
                    //{
                    //    if (i == 0)
                    //    {
                    //        obs_names.Add(obs[0][0].Trim());
                    //        n_points_under_a_name[0] = 1;
                    //        k = 0;
                    //    }
                    //    else
                    //    {
                    //        name = obs[i][0].Trim();
                    //        if (!obs_names.Contains(name))
                    //        {
                    //            obs_names.Add(obs[i][0].Trim());
                    //            k = k + 1;
                    //            n_points_under_a_name[k] = 1;
                    //        }
                    //        else
                    //            n_points_under_a_name[k]++;
                    //    }
                    //}

                    ////number of observation points 
                    //w.WriteLine(obs_names.Count);

                    //name = "";
                    //k = 0;
                    //for (int j = 0; j < obs.Count; ++j)
                    //{
                    //    if (!(name == obs[j][0].Trim()))
                    //    {
                    //        //observation point name
                    //        w.WriteLine(obs[j][0]);
                    //        name = obs[j][0].Trim();
                    //        //Number of points in this group
                    //        w.WriteLine(n_points_under_a_name[k]);
                    //        k++; ;
                    //    }

                    //    //X, Y
                    //    w.WriteLine("    " + obs[j][1] + "    " + obs[j][2]);
                    //}
                }
                else
                { // Only one point per name
                    //for (int i = 0; i < number_of_obs; ++i)
                    //{
                    //    if (i == 0)
                    //    {
                    //        obs_names.Add(obs[0][0].Trim());
                    //    }
                    //    else
                    //    {
                    //        name = obs[i][0].Trim();
                    //        if (!obs_names.Contains(name))
                    //        {
                    //            obs_names.Add(obs[i][0].Trim());

                    //        }
                    //    }
                    //}

                    //number of observation points 
                    w.WriteLine(obs.Count);

                    //name = "";
                    for (int j = 0; j < obs.Count; ++j)
                    {
                        //if (!(name == obs[j][0].Trim()))
                        //{
                        //    //observation point name
                        //    w.WriteLine(obs[j][0]);
                        //    name = obs[j][0].Trim();
                        //}

                        //observation point name
                        w.WriteLine(obs[j][0]);
                        //X, Y
                        w.WriteLine("    " + obs[j][1] + "    " + obs[j][2]);
                    }
                }

            }
            catch 
            {
                MessageBox.Show(Universal.Idioma("ERROR 1599110748: error trying to save .OBS file.", "ERROR 1599110748: error leyendo archivo .OBS."), 
                    "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
                
            w.Close();

        }
    }
}

