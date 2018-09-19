using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Simulation_Options {

	class PROFILESFile {
		public int NPROFILES, ND;
		public int[] NVERTICES = new int[3];

        public List<string[]> XYData1 = new List<string[]>();
        public List<string[]> XYData2 = new List<string[]>();
        public List<string[]> XYData3 = new List<string[]>();

        public PROFILESFile() {
            loadData();
        }


        private void loadData()
        {
            //Load all data into the data manager
            DataManager dataManager = DataManager.Instance;
            dataManager.set("NPROFILES", NPROFILES);
            dataManager.set("NDPROFILES", ND);
            dataManager.set("NVERTICES", NVERTICES);
            dataManager.set("PROFILE1DATA", XYData1);
            dataManager.set("PROFILE2DATA", XYData2);
            dataManager.set("PROFILE3DATA", XYData3);
        }

		public void save(string filename) {
			TextWriter w = new StreamWriter(filename);

            DataManager dataManager = DataManager.Instance;
            ND =  Convert.ToInt32(dataManager.get("NDPROFILES"));

            XYData1 = (List<string[]>)dataManager.get("PROFILE1DATA");
            XYData2 = (List<string[]>)dataManager.get("PROFILE2DATA");
            XYData3 = (List<string[]>)dataManager.get("PROFILE3DATA");

            NVERTICES[0] = XYData1.Count;
            NVERTICES[1] = XYData2.Count;
            NVERTICES[2] = XYData3.Count;

            NPROFILES = 0;
            if (NVERTICES[0] > 0) NPROFILES++;
            if (NVERTICES[1] > 0) NPROFILES++;
            if (NVERTICES[2] > 0) NPROFILES++;

			//NPROFILES ND
			w.WriteLine(NPROFILES.ToString() + " " + ND.ToString());

			for(int i = 0; i < 3; ++i)
			{
				//NVERTICES
                if (NVERTICES[i] <= 0) continue;
				w.WriteLine(NVERTICES[i].ToString());

                //X Y
				for (int j = 0; j < NVERTICES[i]; ++j) {
                    if (i == 0) w.WriteLine(Double.Parse(XYData1[j][0]).ToString() + " " + Double.Parse(XYData1[j][1]).ToString());
                    else if (i == 1) w.WriteLine(Double.Parse(XYData2[j][0]).ToString() + " " + Double.Parse(XYData2[j][1]).ToString());
                    else if (i == 2) w.WriteLine(Double.Parse(XYData3[j][0]).ToString() + " " + Double.Parse(XYData3[j][1]).ToString());
				}
			}
			w.Close();

		}

		public bool load(string filename) {

			StreamReader s = File.OpenText(filename);

            //NPROFILES ND
            string read = s.ReadLine();
            string[] split = read.Split(new Char[] { ' ', '\t' });

            NPROFILES = Convert.ToInt32(split[0]);
            ND = Convert.ToInt32(split[1]);

            XYData1.Clear();
            XYData2.Clear();
            XYData3.Clear();

			for(int i=0; i<NPROFILES; ++i) {

				//NVERTICES
				NVERTICES[i] = Convert.ToInt32(s.ReadLine());
				for(int j=0; j<NVERTICES[i]; ++j){

					//X Y
					read = s.ReadLine();
					split = read.Split(new Char[] { ' ', '\t' });
                    //if (i == 0) XYData1.Add(new Double[] { Convert.ToDouble(Convert.ToDouble(split[0]).ToString("F")), Convert.ToDouble(Convert.ToDouble(split[1]).ToString("F")) });
                    //if (i == 1) XYData2.Add(new Double[] { Convert.ToDouble(Convert.ToDouble(split[0]).ToString("F")), Convert.ToDouble(Convert.ToDouble(split[1]).ToString("F")) });
                    //if (i == 2) XYData3.Add(new Double[] { Convert.ToDouble(Convert.ToDouble(split[0]).ToString("F")), Convert.ToDouble(Convert.ToDouble(split[1]).ToString("F")) });
                    if (i == 0) XYData1.Add(new string[] { Convert.ToDouble(split[0]).ToString("N2"), Convert.ToDouble(split[1]).ToString("N2") });
                    if (i == 1) XYData2.Add(new string[] { Convert.ToDouble(split[0]).ToString("N2"), Convert.ToDouble(split[1]).ToString("N2") });
                    if (i == 2) XYData3.Add(new string[] { Convert.ToDouble(split[0]).ToString("N2"), Convert.ToDouble(split[1]).ToString("N2") });


				}
			}

			s.Close();

            loadData();

			return true;
		}

	}

}
