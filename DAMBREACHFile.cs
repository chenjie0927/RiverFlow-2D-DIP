using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Simulation_Options
{
	class DAMBREACHFile : InputFile
	{
		public DAMBREACHFile()
		{
			var emptyDamBreach = new List<string[]>();
			AddVariable("DAMBREACH_VALUES", emptyDamBreach);
			AddVariable("NUMBER_OF_DAMBREACHES", 0);
		}

        public class DAMBREACHES  //Stores a copy of the .DAMBREACH file
        {
          public string DamBreachName;
          public string Coordinates;
          public string Parameters;
          public string SecondaryFileName;
          public int nExtraLines;
          public string[] ExtraData;
        }

        public List<DAMBREACHES> DAMBREACHFileContents = new List<DAMBREACHES>();

        //Load data from file
        public virtual void Load(string DAMBREACHFileName)
	    {
            StreamReader DAMBREACHFile = File.OpenText(DAMBREACHFileName);
            var warnings = new List<string>();

            int index = DAMBREACHFileName.LastIndexOf("\\", StringComparison.Ordinal);
            string DAMBREACHFilePath = DAMBREACHFileName.Remove(index);

            //Read number of dam breaches groups in .DAMBREACH file:
            string nGroups = DAMBREACHFile.ReadLine();
            int numberOfGroups = Convert.ToInt32(nGroups);
            AddVariable("NUMBER_OF_DAMBREACHES", numberOfGroups);

            //Reset data structs to store .DAMBREACH data in local memory:
            var mainTable = new List<string[]>();
            DAMBREACHFileContents.Clear();

            //
            //(Code space for individual variables)
            //

            for (int i = 0; i < numberOfGroups; ++i)
            { //Read all rest of data groups from .DAMBREACH file....
                try
                {
                    //Dam Breach name:
                    string damBreachName = DAMBREACHFile.ReadLine().Trim();
                    //X0, Y0:
                    string coordinates = DAMBREACHFile.ReadLine().Trim();
                    string[] split = coordinates.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    string X0 = split[0];
                    string Y0 = split[1];
                    //Crest z crest, beach angle, cofficient:
                    string parameters = DAMBREACHFile.ReadLine();
                    split = parameters.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    string zCrest = split[0];
                    string breachAngle = split[1]; ;
                    string coeff = split[2];        
 
                    //Secondary table (dam breach evolution) file name:
                    string secondaryTableFileName = DAMBREACHFile.ReadLine().Trim();

                    //Read extra lines:
                    int nExtraLines = Convert.ToInt32(DAMBREACHFile.ReadLine().Trim());
                    var extras = new string[nExtraLines];
                    for (var ii = 0; ii < nExtraLines; ++ii)
                    {
                        extras[ii] = DAMBREACHFile.ReadLine();
                    }

                    //Create this group from data just read:
                    var group = new DAMBREACHES();
                    group.DamBreachName = damBreachName;
                    group.Coordinates = coordinates;
                    group.Parameters = parameters;
                    group.SecondaryFileName = secondaryTableFileName;
                    group.nExtraLines = nExtraLines;
                    group.ExtraData = extras;

                    //Store data in memory for this apenings group:
                    DAMBREACHFileContents.Add(group);

                    //Store in mainTable a  row to be shown in dam breach table in DIP tab: 
                    //(dam breach name, z crest, angle, coeff, file name)
                    var row = new string[7];
                    row[0] = damBreachName;
                    row[1] = X0;
                    row[2] = Y0;
                    row[3] = zCrest;
                    row[4] = breachAngle;
                    row[5] = coeff;
                    row[6] = secondaryTableFileName;

                    mainTable.Add(row);

                    //Potential new temporal evolution group (secundary dependant table). evolution (time, width, height) is 
                    // stored in a file whose name could be repeated in the .DAMBREACH file for another dam breach. Only one copy of the file contents is 
                    //stored in memory in SecondaryGroups structure. If modified, only that copy is changed. 
                    //It will be permanently stored when the user clicks "Save .DAMBREACH".

                    Universal.LoadSecondaryTable("DAMBREACH", DAMBREACHFilePath + "\\" + secondaryTableFileName, ref warnings);


                }
                catch (Exception ex)
                {
                    MessageBox.Show(Universal.Idioma("ERROR 1206171118: error while proccessing  ", "ERROR 1206171118: error leyendo  ") + 
                        DAMBREACHFileName + ". " +
                                    Environment.NewLine + ex.Message, "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            DAMBREACHFile.Close();

            //Save variables associated to control tags:
            AddVariable("DAMBREACH_VALUES", mainTable);

            //
            //
            //  Empty lines left intentionally

            if (warnings.Count > 0)
            {
                string warningsList = Universal.Idioma("WARNING 1206171119: The following dam breach files do not exist: \n\n",
                                                        "ADVERTENCIA 1206171119: Los siguientes archivos de brechas presas no existen : \n\n");
                for (int i = 0; i < warnings.Count; i++)
                {
                    warningsList += "   ° " + warnings[i] + "\n";
                }
                    warningsList += Universal.Idioma("\nDefault files were created.", "\nAchivos por defecto fueron creados.");
                    MessageBox.Show(warningsList, "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

		    //Save GATES data to file.
	    public virtual void Save(string DAMBREACHFileName)
	    {
          try
          {
            if (DAMBREACHFileContents.Count > 0)
            {        
              {//Write .DAMBREACH file:
                TextWriter w = new StreamWriter(DAMBREACHFileName);
                //number of breaches::
                w.WriteLine(DAMBREACHFileContents.Count);




                for (int i = 0; i < DAMBREACHFileContents.Count; ++i)
                {
                  //dam breach name, coordinates, parameters, filename, rows 
                  w.WriteLine(DAMBREACHFileContents[i].DamBreachName);
                  w.WriteLine(DAMBREACHFileContents[i].Coordinates);
                  w.WriteLine(DAMBREACHFileContents[i].Parameters);
                  w.WriteLine(DAMBREACHFileContents[i].SecondaryFileName);
                  w.WriteLine(DAMBREACHFileContents[i].nExtraLines);
                  for (int j = 0; j < DAMBREACHFileContents[i].nExtraLines; ++j)
                  {
                    w.WriteLine(DAMBREACHFileContents[i].ExtraData[j]);
                  }
                }
                w.Close();
              }

                //Write secondary temporal evolution data files. 
                int nColumns = 3;
                Universal.SaveSecondaryTables("DAMBREACH", nColumns);
            }
            else
            {
              MessageBox.Show(Universal.Idioma("The dam breach table is empty. It was not saved.","La tabla de brechas esá vacía. No fue almacanada."), 
                  "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
          }
          catch
          {
            MessageBox.Show(Universal.Idioma("ERROR 1206171122: error while saving .DAMBREACH file.", "ERROR 1206171122: error almacenando archivo .DAMBREACH file."), 
                
                "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
        }

        //private static void LoadDamBreachSecondaryData(string secondaryFileName, ref List<string> warnings)
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
        //        thisGroup.Component = "GATES";
        //        thisGroup.FileName = secondaryFileName;
        //        thisGroup.NPoints = Convert.ToInt64(secondaryFile.ReadLine());
        //        var thisTable = new Universal.ExtraData[thisGroup.NPoints];

        //        //Read every line with 2 values: Time, Aperture
        //        for (int j = 0; j <= thisGroup.NPoints - 1; ++j)
        //        {
        //          string next = secondaryFile.ReadLine();
        //          string[] splt = next.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        //          thisTable[j].Col0 = splt[0];
        //          thisTable[j].Col1 = splt[1];
        //        }

        //        secondaryFile.Close();

        //        //Store apertures table:
        //        thisGroup.Table = thisTable;

        //        //Store this new group of apertures:
        //        Universal.SecondaryGroups.Add(secondaryFileName, thisGroup);
        //      }
        //      catch (Exception ex)
        //      {
        //        MessageBox.Show(Universal.Idioma("ERROR 2404171654: error reading file " + secondaryFileName + ". " + ex.Message, "RiverFlow2D", MessageBoxButtons.OK,
        //                        MessageBoxIcon.Error);
        //      }
        //    }
        //    else
        //    {
        //      //Gates apertures file doesn't exist in memory.
        //      Universal.CreateSecondaryDefaultFile("GATES", secondaryFileName, 2);
        //      if (warnings.IndexOf(secondaryFileName) == -1)
        //      {
        //        warnings.Add(secondaryFileName);
        //      }
        //    }

        //  }
        //}
  }
}
