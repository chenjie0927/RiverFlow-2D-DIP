using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Simulation_Options
{
	class WINDPLUSFile : InputFile
	{
		public WINDPLUSFile()
		{
			var emptyWidPlus = new List<string[]>();
			AddVariable("WINDPLUSZONES_VALUES", emptyWidPlus);
			AddVariable("NUMBER_OF_WINDPLUS_ZONES", 0);
			AddVariable("WINDPLUS_AIRDENSITY", 1.2);
			AddVariable("WINDPLUS_STRESSCOEFF_CD", 0.001);
		}

		public class WIND  //Stores a copy of the .WIND file
    {
			public string SecondaryFileName;
			public int nExtraLines;
			public string[] ExtraData;
		};

		public List<WIND> WINDFileContents = new List<WIND>();

    // Load .WIND data from file.
        public virtual void Load(string WINDPLUSFileName)
        {
            StreamReader WINDFile = File.OpenText(WINDPLUSFileName);
            var warnings = new List<string>();

            int index = WINDPLUSFileName.LastIndexOf("\\", StringComparison.Ordinal);
            string WINDPLUSFilePath = WINDPLUSFileName.Remove(index);

            //Number of wind groups in .WIND file:
            string nGroups = WINDFile.ReadLine();
            int numberOfGroups = Convert.ToInt32(nGroups);
            AddVariable("NUMBER_OF_WINDS", numberOfGroups);

            //Reset data structs to store .WIND data in local memory:
            var mainTable = new List<string[]>();
            WINDFileContents.Clear();

            //2 values not contained in WINDFIleContents:
            string windPlusWindCoeff = WINDFile.ReadLine();
            string windPlusAirDensity = WINDFile.ReadLine();

            for (int i = 0; i < numberOfGroups; ++i)
            { //Read all rest of data from .WIND file....
                try
                {
                    //
                    //
                    //
                    //
                    // 
                    //
                    //
                    //
                    //  Empty lines left intentionally       

                    //Secondary table (Wind velocities table) file name:
                    string secondaryTableFileName = WINDFile.ReadLine().Trim();

                    //Read extra lines:
                    int nExtraLines = Convert.ToInt32(WINDFile.ReadLine().Trim());
                    var extras = new string[nExtraLines];
                    for (int ii = 0; ii < nExtraLines; ++ii)
                    {
                    extras[ii] = WINDFile.ReadLine();
                    }

                    //Create this group from data just read:
                    var group = new WIND();
                    group.SecondaryFileName = secondaryTableFileName;
                    group.nExtraLines = nExtraLines;
                    group.ExtraData = extras;



                    //Store data of this zone:
                    WINDFileContents.Add(group);

                    //Store in mainTable a row to be shown in zones table in DIP tab:
                    //(index, wind velocities name)
                    var row = new string[2];
                    row[0] = i.ToString();
                    row[1] = secondaryTableFileName;




                    mainTable.Add(row);

                    //Potential new velocities group (secundary dependant table). Velocities data (time, Ux, Uy) is stored in
                    //a file whose name could be repeated ni the .WIND file for another zone. Only one copy of the file contents is 
                    //stored in memory in MyWind structure. If modified, only that copy is changed. It is permanently stored when
                    //the user clicks "Save .WIND".

                    Universal.LoadSecondaryTable("WIND", WINDPLUSFilePath + "\\" + secondaryTableFileName, ref warnings);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(Universal.Idioma("ERROR 1803171736: error while proccessing  ", "ERROR 1803171736: error procesando  ") + 
                        WINDPLUSFileName + ". " +
                                    Environment.NewLine + ex.Message, "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            WINDFile.Close();

            //Save variables associated to control tags:
            double windCoeff = Convert.ToDouble(windPlusWindCoeff);
            double airDensity = Convert.ToDouble(windPlusAirDensity);
            AddVariable("WINDPLUS_STRESSCOEFF_CD", windCoeff);
            AddVariable("WINDPLUS_AIRDENSITY", airDensity);
            AddVariable("WINDPLUSZONES_VALUES", mainTable);

            if (warnings.Count > 0)
            {
            string warningsList = Universal.Idioma("WARNING 0811121034: The following wind files do not exist: \n\n", "WARNING 0811121034: Los siguientes archivos no existen: \n\n") ;
            for (int i = 0; i < warnings.Count; i++)
            {
                warningsList += "   ° " + warnings[i] + "\n";
            }
            warningsList += Universal.Idioma("\nDefault files were created.", "\nAchivos por defecto fueron creados.");
            MessageBox.Show(warningsList, "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

    //Save WIND data to file
        public virtual void Save(string WINDPLUSFileName)
        {
            try
            {
                if (WINDFileContents.Count > 0)
                {
                    { //Write .WIND file:
                        TextWriter w = new StreamWriter(WINDPLUSFileName);
                        //number of wind zones and parameters:
                        w.WriteLine(WINDFileContents.Count);
                        w.WriteLine(GetVariable("WINDPLUS_STRESSCOEFF_CD"));
                        w.WriteLine(GetVariable("WINDPLUS_AIRDENSITY"));

                        for (int i = 0; i < WINDFileContents.Count; ++i)
                        {
                            //wind file name, rows
                            w.WriteLine(WINDFileContents[i].SecondaryFileName);
                            w.WriteLine(WINDFileContents[i].nExtraLines);
                            for (int j = 0; j < WINDFileContents[i].nExtraLines; ++j)
                            {
                            w.WriteLine(WINDFileContents[i].ExtraData[j]);
                            }
                        }
                        w.Close();
                    }

                    // Write secondary wind data files.
                    int nColumns = 3;
                    Universal.SaveSecondaryTables("WIND", nColumns);

                    //foreach (Universal.SecondaryGroup aGroup in Universal.SecondaryGroups.Values)
                    //{
                    //    if (aGroup.Component == "WIND")
                    //    {
                    //        TextWriter secondaryFile = new StreamWriter(aGroup.FileName);

                    //        secondaryFile.WriteLine(aGroup.NPoints);
                    //        for (int i = 0; i < aGroup.NPoints; i++)
                    //        {
                    //            secondaryFile.WriteLine(aGroup.Table[i].Col0 + "   " +
                    //                                aGroup.Table[i].Col1 + "   " +
                    //                                aGroup.Table[i].Col2);
                    //        }
                    //        secondaryFile.Close();
                    //    }
                    //}
                }
            else
            {
                MessageBox.Show(Universal.Idioma("The wins table is empty. It was not saved.", "La tabla de viento está vacía. No fue almacenada."), 
                    "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            }
            catch
            {
            MessageBox.Show(Universal.Idioma("ERROR 1903170641: error while saving .WIND file.", "ERROR 1903170641: error almacenando archivo .WIND."), 
                "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    //private static void LoadWindSecundaryData(string secondaryFileName, ref List<string> warnings)
    //{
    //  //Create new item in SecondaryGroups structure (if it is not 
    //  //already there) with data from secondaryFileName.

    //  if (Universal.SecondaryGroups.ContainsKey(secondaryFileName))
    //  {
    //    //Do not load the data from this file since it is already in memory.
    //    //In memory it could have been modified but we want to keep it as it 
    //    //is, just as Word and Excel does when a file is loaded again: 
    //    //it keeps the modifications.
    //  }
    //  else //Load this file to SecondaryGroups.
    //  {
    //    if (File.Exists(secondaryFileName))
    //    {
    //      try
    //      {
    //        StreamReader secondaryFile;
    //        secondaryFile = File.OpenText(secondaryFileName);

    //        var thisGroup = new Universal.SecondaryGroup();
    //        thisGroup.Component = "WIND";
    //        thisGroup.FileName = secondaryFileName;
    //        thisGroup.NPoints = Convert.ToInt64(secondaryFile.ReadLine());
    //        var thisTable = new Universal.SecondaryTable[thisGroup.NPoints];

    //        //Read every line with 3 values: Time, Ux, UY
    //        for (int j = 0; j <= thisGroup.NPoints - 1; ++j)
    //        {
    //          string next = secondaryFile.ReadLine();
    //          string[] splt = next.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
    //          thisTable[j].Col0 = splt[0];
    //          thisTable[j].Col1 = splt[1];
    //          thisTable[j].Col2 = splt[2];
    //        }

    //        secondaryFile.Close();

    //        //Store velocities table:
    //        thisGroup.Table = thisTable;

    //        //Store this new group of velocities:
    //        Universal.SecondaryGroups.Add(secondaryFileName, thisGroup);
    //      }
    //      catch (Exception ex)
    //      {
    //        MessageBox.Show(Universal.Idioma("ERROR 1703172030: error reading file " + secondaryFileName + ". " + ex.Message, "RiverFlow2D", MessageBoxButtons.OK,
    //                        MessageBoxIcon.Error);
    //      }
    //    }
    //    else
    //    {
    //      //Wind file doesn't exist in memory.
    //      Universal.CreateSecondaryDefaultFile("WIND", secondaryFileName, 3);
    //      if (warnings.IndexOf(secondaryFileName) == -1)
    //      {
    //        warnings.Add(secondaryFileName);
    //      }
    //    }
    //  }
    //}
  }
}


