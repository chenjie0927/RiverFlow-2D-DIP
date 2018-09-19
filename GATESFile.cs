using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Simulation_Options
{
	class GATESFile : InputFile
	{
		public GATESFile()
		{
			var emptyGates = new List<string[]>();
			AddVariable("GATES_VALUES", emptyGates);
			AddVariable("NUMBER_OF_GATES", 0);
		}

    public class GATES  //Stores a copy of the .GATES file
    {
      public string GateName;
      public string Parameters;
      public string SecondaryFileName;
      public int nExtraLines;
      public string[] ExtraData;
    }

    public List<GATES> GATESFileContents = new List<GATES>();

    //Load data from file
    public virtual void Load(string GATESFileName)
	{
      StreamReader GATESFile = File.OpenText(GATESFileName);
      var warnings = new List<string>();

      int index = GATESFileName.LastIndexOf("\\", StringComparison.Ordinal);
      string GATESFilePath = GATESFileName.Remove(index);

      //Read number of gate groups in .GATES file:
      string nGroups = GATESFile.ReadLine();
      int numberOfGroups = Convert.ToInt32(nGroups);
      AddVariable("NUMBER_OF_GATES", numberOfGroups);

      //Reset data structs to store .GATES data in local memory:
      var mainTable = new List<string[]>();
      GATESFileContents.Clear();

      //
      //(Code space for individual variables)
      //

      for (int i = 0; i < numberOfGroups; ++i)
      { //Read all rest of data groups from .GATES file....
        try
        {
          //Gate name:
          string gateName = GATESFile.ReadLine().Trim();

          //Crest elevation, gate height, and Cd.
          string parameters = GATESFile.ReadLine();
          string[] split = parameters.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
          string crestElevation = split[0];
          string gateHeight = split[1]; ;
          string Cd = split[2];        
 
          //Secondary table (Gates openings table) file name:
          string secondaryTableFileName = GATESFile.ReadLine().Trim();

          //Read extra lines:
          int nExtraLines = Convert.ToInt32(GATESFile.ReadLine().Trim());
          var lines = new string[nExtraLines];
          for (var ii = 0; ii < nExtraLines; ++ii)
          {
            lines[ii] = GATESFile.ReadLine();
          }

          //Create this group from data just read:
          var group = new GATES();
          group.GateName = gateName;
          group.Parameters = parameters;
          group.SecondaryFileName = secondaryTableFileName;
          group.nExtraLines = nExtraLines;
          group.ExtraData = lines;

          //Store data in memory for this apenings group:
          GATESFileContents.Add(group);

          //Store in mainTable a  row to be shown in gates table in DIP tab: 
          //(gate name, crest elevation, gate height, Cd, openings table name)
          var row = new string[5];
          row[0] = gateName;
          row[1] = crestElevation;
          row[2] = gateHeight;
          row[3] = Cd;
          row[4] = secondaryTableFileName;

          mainTable.Add(row);

          //Potential new openings group (secundary dependant table). Openings (time, aperture) is 
          // stored in a file whose name could be repeated in the .GATES file for another gate. Only one copy of the file contents is 
          //stored in memory in OpeningsFiles (of GATEOpenings structure). If modified, only that copy is changed. 
          //It will be permanently stored when the user clicks "Save .GATES".

          Universal.LoadSecondaryTable("GATES", GATESFilePath + "\\" + secondaryTableFileName, ref warnings);

        }
        catch (Exception ex)
        {
          MessageBox.Show(Universal.Idioma("ERROR 2304171526: error while proccessing  ", "ERROR 2304171526: error procesando  ") + 
              GATESFileName + ". " +
                            Environment.NewLine + ex.Message, "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }

      GATESFile.Close();

      //Save variables associated to control tags:
      AddVariable("GATES_VALUES", mainTable);

      //
      //
      //  Empty lines left intentionally

      if (warnings.Count > 0)
      {
        string warningsList = Universal.Idioma("WARNING 2003171725: The following gates files do not exist: \n\n", "WARNING 2003171725: Los siguientes archivos no existen: \n\n");
        for (int i = 0; i < warnings.Count; i++)
        {
          warningsList += "   ° " + warnings[i] + "\n";
        }
        warningsList += Universal.Idioma("\nDefault files were created.", "\nAchivos por defecto fueron creados.");
        MessageBox.Show(warningsList, "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }

	//Save GATES data to file.
	public virtual void Save(string GATESFileName)
	{
      try
      {
        if (GATESFileContents.Count > 0)
        {        
          {//Write .GATES file:
            TextWriter w = new StreamWriter(GATESFileName);
            //number of gates::
            w.WriteLine(GATESFileContents.Count);




            for (int i = 0; i < GATESFileContents.Count; ++i)
            {
              //Gate name, parameters, filename, rows 
              w.WriteLine(GATESFileContents[i].GateName);
              w.WriteLine(GATESFileContents[i].Parameters);
              w.WriteLine(GATESFileContents[i].SecondaryFileName);
              w.WriteLine(GATESFileContents[i].nExtraLines);
              for (int j = 0; j < GATESFileContents[i].nExtraLines; ++j)
              {
                w.WriteLine(GATESFileContents[i].ExtraData[j]);
              }
            }
            w.Close();
          }

        //Write secondary openings data files. 
        int nColumns = 2;
        Universal.SaveSecondaryTables("GATES", nColumns);

          //foreach (Universal.SecondaryGroup aGroup in Universal.SecondaryGroups.Values)
          //{
          //  if (aGroup.Component == "GATES")
          //  {
          //    TextWriter secondaryFile = new StreamWriter(aGroup.FileName);

          //    secondaryFile.WriteLine(aGroup.NPoints);

          //    for (int i = 0; i < aGroup.NPoints; i++)
          //    {
          //      secondaryFile.WriteLine(aGroup.Table[i].Col0 + "   " +
          //                        aGroup.Table[i].Col1);
          //    }
          //    secondaryFile.Close();
          //  }
          //}
        }
        else
        {
          MessageBox.Show(Universal.Idioma("The gates table is empty. It was not saved.","La tabla de compuertas está vacía. No fue almacenada"), 
              "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
      }
      catch
      {
        MessageBox.Show(Universal.Idioma("ERROR 2504170705: error while saving .GATES file.", "ERROR 2504170705: error almacenando .GATES file."), 
            "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private static void LoadGatesSecondaryData(string secondaryFileName, ref List<string> warnings)
    {
      //Create new item in SecondaryGroups structure (if it is not 
      //already there) with data from secondaryFileName.

      if (Universal.SecondaryGroups.ContainsKey(secondaryFileName))
      {
        //Do not load the data from this file since it is already in memory.
        //In memory it could have been modified but we want to keep it as it 
        //is, just as Word and Excel does when a file is loaded again: 
        //it keeps the modifications.
      }
      else //Load this file to SecondaryGroups.
      {
        if (File.Exists(secondaryFileName))
        {
          try
          {
            StreamReader secondaryFile;
            secondaryFile = File.OpenText(secondaryFileName);

            var thisGroup = new Universal.SecondaryGroup();
            thisGroup.Component = "GATES";
            thisGroup.FileName = secondaryFileName;
            thisGroup.NPoints = Convert.ToInt64(secondaryFile.ReadLine());
            var thisTable = new Universal.SecondaryTable[thisGroup.NPoints];

            //Read every line with 2 values: Time, Aperture
            for (int j = 0; j <= thisGroup.NPoints - 1; ++j)
            {
              string next = secondaryFile.ReadLine();
              string[] splt = next.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
              thisTable[j].Col0 = splt[0];
              thisTable[j].Col1 = splt[1];
            }

            secondaryFile.Close();

            //Store apertures table:
            thisGroup.Table = thisTable;

            //Store this new group of apertures:
            Universal.SecondaryGroups.Add(secondaryFileName, thisGroup);
          }
          catch (Exception ex)
          {
            MessageBox.Show(Universal.Idioma("ERROR 2404171654: error reading file ", "ERROR 2404171654: error leyando archivo ") + 
                secondaryFileName + ". " + ex.Message, "RiverFlow2D", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
          }
        }
        else
        {
          //Gates apertures file doesn't exist in memory.
          Universal.CreateSecondaryDefaultFile("GATES", secondaryFileName, 2);
          if (warnings.IndexOf(secondaryFileName) == -1)
          {
            warnings.Add(secondaryFileName);
          }
        }

      }
    }
  }
}
