using System;
using System.IO;
using System.Windows.Forms;

namespace Simulation_Options
{
    class FEDFile : ConfiguratedFile
    {
        #region Region of old code for loading and saving .FED files (if used in the future, needs to be updated to new file format)
        //public int[] N_NODES = new int[2];

        //public List<string[]> NodeData1 = new List<string[]>();
        //public List<string[]> NodeData2 = new List<string[]>();

        //public string[] split;

        //public FEDFile()
        //{
        //    loadData();
        //}


        //private void loadData()
        //{
        //    //Load all data into the data manager

        //    DataManager dataManager = DataManager.Instance;

        //    dataManager.set("NELEMS", N_NODES[0]);
        //    dataManager.set("NNODES", N_NODES[1]);
        //    dataManager.set("MESHNODESDATA", NodeData1);
        //    dataManager.set("MESHCONECTIVITYDATA", NodeData2);


        //}

        //public void save(string filename)
        //{
        //    TextWriter w = new StreamWriter(filename);

        //    string newLine;
        //    DataManager dataManager = DataManager.Instance;

        //    NodeData1 = (List<string[]>)dataManager.get("MESHNODESDATA");
        //    NodeData2 = (List<string[]>)dataManager.get("MESHCONECTIVITYDATA");

        //    newLine = Convert.ToString(N_NODES[0]) + "   " + Convert.ToString(N_NODES[1]);

        //    w.WriteLine(newLine);

        //    for (int i = 0; i < 2; ++i)
        //    {

        //        for (int j = 0; j < N_NODES[i]; ++j)
        //        {

        //            if (i == 0)
        //                newLine = NodeData1[j][0];
        //            else
        //                newLine = NodeData2[j][0];

        //            w.WriteLine(newLine);
        //        }
        //    }
        //    w.Close();

        //}

        //public bool load(string filename)
        //{

        //    StreamReader s = File.OpenText(filename);
        //    string read;

        //    NodeData1.Clear();
        //    NodeData2.Clear();

        //    read = s.ReadLine().Trim();

        //    split = read.Split(new Char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

        //    N_NODES[0] = Convert.ToInt32(split[0]);
        //    N_NODES[1] = Convert.ToInt32(split[1]);

        //    for (int i = 0; i < 2; ++i)
        //    {

        //        for (int j = 0; j < N_NODES[i]; ++j)
        //        {
        //            read = s.ReadLine().Trim();
        //            split = read.Split(new Char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);


        //            if (i == 0)
        //                NodeData1.Add(new string[] { Convert.ToDouble(split[0]).ToString("N2"), Convert.ToDouble(split[1]).ToString("N2"), 
        //                                             Convert.ToDouble(split[2]).ToString("N2"), Convert.ToDouble(split[3]).ToString("N2"),
        //                                             Convert.ToDouble(split[4]).ToString("N2"),Convert.ToDouble(split[5]).ToString("N2")});
        //            else
        //                NodeData2.Add(new String[] { read });
        //        }
        //    }


        //    s.Close();

        //    loadData();

        //    return true;
        //} 
        #endregion for old code for loading and saving .FED files

        //read file names
        public string[] ReadFileNames(string filename)
        {
	        StreamReader s = File.OpenText(filename);
            string line = s.ReadLine();
            string[] split = line.Split(new Char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            int numberOfElements = Convert.ToInt32(split[0]);
            int numberOfNodes = Convert.ToInt32(split[1]);

            var fileNames = new string[numberOfNodes];

            try
            {
                for (int i = 0; i < numberOfNodes; ++i)
                {
                    //node, X, Y, initial bed elevation, initial water elevation, max erosion depth, boundary condition type, file name      
                    line = s.ReadLine();
                    split = line.Split(new Char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    fileNames[i] = split[6];
                }

            }
            catch
            {
                MessageBox.Show(Universal.Idioma("ERROR 0601120956: error trying to read .FED file.", "ERROR 0601120956: error leyendo archivo .FED."), 
                    "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            s.Close();

            return fileNames;

        }



    }
}
