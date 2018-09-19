using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Simulation_Options
{
	class LRAINFile : InputFile
	{
	    public LRAINFile()
	    {
		    var emptyRainEvap = new List<string[]>();
		    AddVariable("RAINEVAPZONES_VALUES", emptyRainEvap);
		    AddVariable("NUMBER_OF_RAINEVAP_ZONES", 0);
	    }

        public class LRAIN  //Stores a copy of the .LRAIN file
	    {
		    public string SecondaryFileName;
		    public int nExtraLines;
		    public string[] ExtraData;
	    };

	    public List<LRAIN> LRAINFileContents = new List<LRAIN>();

	    //Load .LRAIN data from file
	    public virtual void Load(string LRAINFileName)
	    {
		    StreamReader RAINFile = File.OpenText(LRAINFileName);
		    var warnings = new List<string>();

		    int index = LRAINFileName.LastIndexOf("\\", StringComparison.Ordinal);
		    string LRAINFilePath = LRAINFileName.Remove(index);

		    //Number of rain/evaporation groups in .LRAIN file:
		    string nGroups = RAINFile.ReadLine();
		    int numberOfGroups = Convert.ToInt32(nGroups);
            if (numberOfGroups > 0)
            {
                AddVariable("NUMBER_OF_RAINEVAP_ZONES", numberOfGroups);

                //Reset data structs to store .LRAIN data in local memory:
                var mainTable = new List<string[]>();
                LRAINFileContents.Clear();

                //
                //(Code space for individual variables)
                //

                for (int i = 0; i < numberOfGroups; ++i)
                { //Read all rest of data from .LRAIN file....
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

                        //Secondary table (Rain/evaporation table) file name:
                        string secondaryTableFileName = RAINFile.ReadLine().Trim();

                        //Read extra lines:
                        int nExtraLines = Convert.ToInt32(RAINFile.ReadLine().Trim());
                        var extras = new string[nExtraLines];
                        for (int ii = 0; ii < nExtraLines; ++ii)
                        {
                            extras[ii] = RAINFile.ReadLine();
                        }

                        //Create this group from data just read:
                        var group = new LRAIN();
                        group.SecondaryFileName = secondaryTableFileName;
                        group.nExtraLines = nExtraLines;
                        group.ExtraData = extras;



                        //Store data of this zone:
                        LRAINFileContents.Add(group);

                        //Store in mainTable a row to be shown in zones table in DIP tab: 
                        //(index, rain/evaporation hydrograph)
                        var row = new string[2];
                        row[0] = i.ToString();
                        row[1] = secondaryTableFileName;




                        mainTable.Add(row);

                        //Potential new rain/evaporation group (secundary dependant table). Rain/evaporation data hydrograph (time, Rainfall, Evaporation) is 
                        // stored in a file whose name could be repeated in the .LRAIN file for another zone. Only one copy of the file contents is 
                        //stored in memory in SecondaryGroups structure. If modified, only that copy is changed. It is permanently stored when 
                        //the user clicks "Save .LRAIN".

                        Universal.LoadSecondaryTable("LRAIN", LRAINFilePath + "\\" + secondaryTableFileName, ref warnings);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(Universal.Idioma("ERROR 2003171649: error while proccessing  ", "ERROR 2003171649: error procesando  ") + 
                            LRAINFileName + ". " +
                                        Environment.NewLine + ex.Message, "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                

                //Save variables associated to control tags:
                AddVariable("RAINEVAPZONES_VALUES", mainTable);
                //
                //
                //
                //  Empty lines left intentionally

                if (warnings.Count > 0)
                {
                    string warningsList = Universal.Idioma("WARNING 2003171725: The following rain/evaporation files do not exist: \n\n", "WARNING 2003171725: Los siguientes archivos no existen: \n\n");
                    for (int i = 0; i < warnings.Count; i++)
                    {
                        warningsList += "   ° " + warnings[i] + "\n";
                    }
                    warningsList += Universal.Idioma("\nDefault files were created.", "\nAchivos por defecto fueron creados.");
                    MessageBox.Show(warningsList, "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                //Number of zones (groups) is negative => Rainfall/evaporation modelled from .RFC catalogue.
                    //MessageBox.Show("Rainfall/Evaporation will be modelled from .RFC catalogue.", "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            RAINFile.Close();
	    }

	    //Save .LRAIN data to file
	    public virtual void Save(string LINFFileName)
	    {
		    try
		    {
		    if (LRAINFileContents.Count > 0)
		    {

			    {//Write .LRAIN file:
				    TextWriter w = new StreamWriter(LINFFileName);
				    //number of rain/evaporation zones and parameters:
				    w.WriteLine(LRAINFileContents.Count);


				    for (int i = 0; i < LRAINFileContents.Count; ++i)
				    {
					    w.WriteLine(LRAINFileContents[i].SecondaryFileName);
					    w.WriteLine(LRAINFileContents[i].nExtraLines);
					    for (int j = 0; j < LRAINFileContents[i].nExtraLines; ++j)
					    {
					    w.WriteLine(LRAINFileContents[i].ExtraData[j]);
					    }
				    }
				    w.Close();
			    }

                    // Write secondary rain/evaporation data files.
                    int nColumns = 3;
                    Universal.SaveSecondaryTables("LRAIN", nColumns);

                    //foreach (Universal.SecondaryGroup aGroup in Universal.SecondaryGroups.Values)
                    //{
                    //    if (aGroup.Component == "LRAIN")
                    //    {
                    //	    TextWriter secondaryFile = new StreamWriter(aGroup.FileName);

                    //	    secondaryFile.WriteLine(aGroup.NPoints);
                    //	    for (int i = 0; i < aGroup.NPoints; i++)
                    //	    {
                    //	    secondaryFile.WriteLine(aGroup.Table[i].Col0 + "   " +
                    //						    aGroup.Table[i].Col1 + "   " +
                    //						    aGroup.Table[i].Col2);
                    //	    }
                    //	    secondaryFile.Close();
                    //    }
                    //}
                }
		    else
		    {
			    MessageBox.Show(Universal.Idioma("The rain/evaporation table is empty. It was not saved.", 
                    "La tabla de lluvia/evaporación está vacía. No fue almacenada"), 
                    "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Information);
		    }
		    }
		    catch
		    {
		    MessageBox.Show(Universal.Idioma("ERROR 2103170805: error while saving .LRAIN file.", "ERROR 2103170805: error almacenando .LRAIN."), 
                "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
		    }

	    }

	    //private static void LoadRainSecondaryData(string secondaryFileName, ref List<string> warnings)
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
	    //        thisGroup.Component = "LRAIN";
	    //        thisGroup.FileName = secondaryFileName;
	    //        thisGroup.NPoints = Convert.ToInt64(secondaryFile.ReadLine());
	    //        var thisTable = new Universal.ExtraData[thisGroup.NPoints];

	    //        //Read every line with 3 values: Time, Rainfall, Evaporation
	    //        for (int j = 0; j <= thisGroup.NPoints - 1; ++j)
	    //        {
	    //          string next = secondaryFile.ReadLine();
	    //          string[] splt = next.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
	    //          thisTable[j].Col0 = splt[0];
	    //          thisTable[j].Col1 = splt[1];
	    //          thisTable[j].Col2 = splt[2];
	    //        }

	    //        secondaryFile.Close();

	    //        //Store rain/evaporation table:
	    //        thisGroup.Table = thisTable;

	    //        //Store this new group of rain/evaporation:
	    //        Universal.SecondaryGroups.Add(secondaryFileName, thisGroup);
	    //      }
	    //      catch (Exception ex)
	    //      {
	    //        MessageBox.Show(Universal.Idioma("ERROR 2103170825: error reading file " + secondaryFileName + ". " + ex.Message, "RiverFlow2D", MessageBoxButtons.OK,
	    //                        MessageBoxIcon.Error);
	    //      }
	    //    }
	    //    else
	    //    {
	    //      //Rain/evaporation file doesn't exist in memory.
	    //      Universal.CreateSecondaryDefaultFile("LRAIN", secondaryFileName, 3);
	    //      if (warnings.IndexOf(secondaryFileName) == -1)
	    //      {
	    //        warnings.Add(secondaryFileName);
	    //      }
	    //    }
	    //  }
	    //}
	}
}

