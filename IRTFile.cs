using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Simulation_Options
{
    class IRTFile : InputFile
    {

        public IRTFile()
        {
            var emptyProfiles = new List<string[]>();
            AddVariable("INTERNALRATINGTABLE_VALUES", emptyProfiles);
            AddVariable("NUMBER_OF_IRTs", 0);
        }

        //Load data from file
        public virtual void Load(string filename)
        {
            try
            {
                //Load all data into the data manager
                StreamReader s = File.OpenText(filename);

	            string read = s.ReadLine();
                int numberOfIrts = Convert.ToInt32(read);

                AddVariable("NUMBER_OF_IRTs", numberOfIrts);

                var internalRatingTables = new List<string[]>();
                for (int i = 0; i < numberOfIrts; ++i)
                {
                    //string[] row = new string[2];

	                //internal rating table name
                    string name = s.ReadLine().Trim();

                    string line = s.ReadLine();
                    string[] split = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    var row = new string[4];

                    row[0] = name;

                    // number of points
                    row[1] = split[0];

                    // boundary condition  type
                    row[2] = split[1];

                    // rating table file name
                    row[3] = split[2];

                    for (int j = 0; j < Convert.ToInt32(row[1]); ++j)
                    {
                        var row2 = new string[5];

                        //Name
                        row2[0] = row[0];
                        //Boundary condition type
                        row2[1] = row[2];
                        //Rating table file name
                        row2[2] = row[3];
                        //x1 y1
                        line = s.ReadLine();
                        split = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        row2[3] = split[0];
                        row2[4] = split[1];

                        internalRatingTables.Add(row2);
                    }
                }

                AddVariable("INTERNALRATINGTABLE_VALUES", internalRatingTables);

                s.Close();
            }
            catch
            {
                MessageBox.Show(Universal.Idioma("ERROR 0212110606: error trying to read .IRT file.", "ERROR 0212110606: error leyendo archivo .IRT."), 
                    "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Save data to file
        public virtual void Save(string filename)
        {
            TextWriter w = new StreamWriter(filename);
	        try
            {
                var internalRatingTables = (List<string[]>)GetVariable("INTERNALRATINGTABLE_VALUES");
                var numberOfRows = (int)GetVariable("NUMBER_OF_IRTs");

	            //number of internal rating tables
                w.WriteLine(numberOfRows);

                // make list of number of points for each rating table
                var numberOfPoints = new int[internalRatingTables.Count];
                string name = internalRatingTables[0][0].Trim();
                int i = 0;

                for (int j = 0; j < internalRatingTables.Count; ++j)
                {   
                    if (name != internalRatingTables[j][0].Trim())
                    {
                        if (j != 0) i += 1;
                        name = internalRatingTables[j][0].Trim();
                        numberOfPoints[i] = 1;
                    }
                    else
                    {
                        numberOfPoints[i] += 1;
                    }
                }


                i = 0;
                for (int j = 0; j < internalRatingTables.Count; ++j)
                {
                    if (name != internalRatingTables[j][0].Trim())
                    {
                        if (j != 0) i += 1;
                        //profiled name
                        w.WriteLine(internalRatingTables[j][0]);
                        name = internalRatingTables[j][0].Trim();
                        //Number of points, boundary condition type, rating table file name
                        w.WriteLine(numberOfPoints[i] + " " + internalRatingTables[j][1] + " " + internalRatingTables[j][2]);
                    }

                    //X, Y
                    w.WriteLine(internalRatingTables[j][3] + " " + internalRatingTables[j][4]);
                }

                
            }
            catch
            {
                MessageBox.Show(Universal.Idioma("ERROR 0312110607: error trying to save .IRT file.", "ERROR 0312110607: error almacenando archivo .IRT."), 
                    "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            w.Close();

        }

        //read file names
        public string[] ReadFileNames(string filename)
        {
            StreamReader s = File.OpenText(filename);
            string read = s.ReadLine();
            int numberOfIrts = Convert.ToInt32(read);

						var fileNames = new string[numberOfIrts];

	        try
	        {
		        for (int i = 0; i < numberOfIrts; ++i)
                {
                    //IRT name
                    s.ReadLine();
                    // number of points, boundary condition type, file name
                    string line = s.ReadLine();
                    string[] split = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    int nPoints = Convert.ToInt32(split[0]);
                    //name of file
                    fileNames[i] = split[2];
                    for (int j =0; j< nPoints;++j)
                        //x y
                        line = s.ReadLine();
                }
	        }
	        catch
            {
                MessageBox.Show(Universal.Idioma("ERROR 1412110939: error trying to read .IRT file.", "ERROR 1412110939: error leyendo archivo .IRT."), 
                    "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            s.Close();

            return fileNames;

        }

    }
}


