using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Simulation_Options
{

    class TBAFile
    {
        public int[] N_NODES = new int[2];

        public List<string[]> NodeData1 = new List<string[]>();
        public List<string[]> NodeData2 = new List<string[]>();

        public string[] split; 

        public TBAFile()
        {
            loadData();
        }


        private void loadData()
        {
            //Load all data into the data manager

            DataManager dataManager = DataManager.Instance;

            dataManager.set("NBOUNDARY1", N_NODES[0]);
            dataManager.set("NBOUNDARY2", N_NODES[1]);
            dataManager.set("BOUNDARYDATA", NodeData1);
            dataManager.set("ISLANDDATA", NodeData2);


        }

        public void save(string filename)
        {
            TextWriter w = new StreamWriter(filename);

            string newLine; 
            DataManager dataManager = DataManager.Instance;

            NodeData1 = (List<string[]>)dataManager.get("BOUNDARYDATA");
            NodeData2 = (List<string[]>)dataManager.get("ISLANDDATA");

            newLine = Convert.ToString(N_NODES[0]) + "   " + Convert.ToString(N_NODES[1]);

            w.WriteLine(newLine);

            for (int i = 0; i < 2; ++i)
            {
                w.WriteLine("-9999");

                for (int j = 0; j < N_NODES[i]; ++j)
                {
                    
                    if (i == 0)
                        newLine = NodeData1[j][0];
                    else
                        newLine = NodeData2[j][0];

                    w.WriteLine(newLine);
                 }
            }
            w.Close();

        }

        public bool load(string filename)
        {

            StreamReader s = File.OpenText(filename);
            string read;

            NodeData1.Clear();
            NodeData2.Clear();

            read = s.ReadLine().Trim();

            split = read.Split(new Char[] { ' ',  '\t' });

            N_NODES[0] = Convert.ToInt32(split[0]);
            for (int k = 0; k < split.Length; ++k)
                {
                    if (split[k] != " " && split[k] != "")
                    {
                        N_NODES[1] = Convert.ToInt32(split[k]);
                    }
                }

            
             
            for (int i = 0; i < 2; ++i)
            { 
                read = s.ReadLine(); //-9999

                for (int j = 0; j < N_NODES[i]; ++j)
                {
                    read = s.ReadLine();
            
                    if (i == 0) 
                        NodeData1.Add(new String[] {read});
                    else 
                        NodeData2.Add(new String[] {read});
                }
            }
            
            s.Close();

            loadData();

            return true;
        }

    }

}
