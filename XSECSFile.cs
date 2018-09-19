using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Simulation_Options
{
    class XSECSFile : InputFile
    {

        public XSECSFile()
        {

            var emptyXsecs = new List<string[]>();
            AddVariable("XSEC_VALUES", emptyXsecs);
            AddVariable("NUMBER_OF_XSECS", 0);

        }

        //Load data from file
        public virtual void Load(string filename)
        {
            //Load all data into the data manager
            StreamReader s = File.OpenText(filename);

            try
            {
	            string read = s.ReadLine();
                int numberOfXsecs = Convert.ToInt32(read);

                AddVariable("NUMBER_OF_XSECS", numberOfXsecs);

                var xsecs = new List<string[]>();
                for (int i = 0; i < numberOfXsecs; ++i)
                {
	                //xsec name
                    string name = s.ReadLine().Trim();

                    string line = s.ReadLine();
                    string[] split = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    var row = new string[3];

                    row[0] = name;
                    // number of points
                    row[1] = split[0];
                    // number of intervals
                    row[2] = split[1];

                    int k = 0;

                    for (int j = 0; k < Convert.ToInt32(row[1]); ++j)
                    {
                        var row2 = new string[6];

                        //Name
                        row2[0] = row[0];

                        //number of intervals
                        row2[1] = row[2];

                        //x1 y1
                        line = s.ReadLine();
                        k++;
                        split = line.Split(new Char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        row2[2] = split[0];
                        row2[3] = split[1];

                        //x2 y2
                        line = s.ReadLine();
                        k++;
                        split = line.Split(new Char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        row2[4] = split[0];
                        row2[5] = split[1];

                        xsecs.Add(row2);
                    }

                }

                AddVariable("XSEC_VALUES", xsecs);

            }
            catch 
            {
                MessageBox.Show(Universal.Idioma("ERROR 1399110843: error trying to read .XSECS file.", "ERROR 1399110843: error leyendo archivo .XSECS."), 
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
                var xsecs = (List<string[]>)GetVariable("XSEC_VALUES");
       
                //int numberOfRows = (int)GetVariable("NUMBER_OF_XSECS");
                //var xsecsNames = new List<string>();
                //new List<int>();

                w.WriteLine(xsecs.Count-1);

                string name = "";

                for (int j = 0; j < xsecs.Count; ++j)
                {
                    if (name != xsecs[j][0].Trim())
                    {
                        if (xsecs[j][0].Trim() != "0.00")
                        {
                            //xsec name
                            w.WriteLine(xsecs[j][0]);
                            name = xsecs[j][0].Trim();
                            //Number of vertices & xsec coeff
                            w.WriteLine("2  " + xsecs[j][1]);

                            //X1, Y1, X2, Y2
                            w.WriteLine("   " + xsecs[j][2] + " " + xsecs[j][3]) ;
                            w.WriteLine("   " + xsecs[j][4] + " " + xsecs[j][5]);

                        }
                    }
                }
            }
            catch 
            {
                MessageBox.Show(Universal.Idioma("ERROR 1399110846: error trying to save .XSECS file.", "ERROR 1399110846: error al almacenar archivo .XSECS."), 
                    "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            w.Close();
        }
    }
}


