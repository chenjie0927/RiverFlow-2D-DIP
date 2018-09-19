using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Simulation_Options
{
	class BRIDGESFile : InputFile
	{
    public BRIDGESFile()
		{
			var emptyBridges = new List<string[]>();
			AddVariable("BRIDGES_VALUES", emptyBridges);
			AddVariable("NUMBER_OF_BRIDGES", 0);
		}

    public class BRIDGES  //Stores a copy of the .BRIDGES file
    {
      public string BridgeName;
      public string Parameters;
      public string SecondaryFileName;
      public int nExtraLines;
      public string[] ExtraData;
    }

    public List<BRIDGES> BRIDGESFileContents = new List<BRIDGES>();

    //Load data from file
    public virtual void Load(string BRIDGESFileName)
    {
      StreamReader BRIDGESFile = File.OpenText(BRIDGESFileName);
      var warnings = new List<string>();

      int index = BRIDGESFileName.LastIndexOf("\\", StringComparison.Ordinal);
      string BRIDGESFilePath = BRIDGESFileName.Remove(index);

      //Read number of bridges groups in .BRIDGES file:
      string nGroups = BRIDGESFile.ReadLine();
      int numberOfGroups = Convert.ToInt32(nGroups);
      AddVariable("NUMBER_OF_BRIDGES", numberOfGroups);

      //Reset data structs to store .BRIDGES data in local memory::
      var mainTable = new List<string[]>();
      BRIDGESFileContents.Clear();

      //
      //(Code space for individual variables)
      //

      for (int i = 0; i < numberOfGroups; ++i)
      { //Read all rest of data groups from .BRIDGES file....
        try
        {
          //Bridge name:
          string bridgeName = BRIDGESFile.ReadLine().Trim();

          //x1, y1, x2, y2.
          string parameters = BRIDGESFile.ReadLine();

          //
          // 
          //
          //  Empty lines left intentionally
        
          //Secondary table (Bridge geometry profiles table) file name:
          string secondaryTableFileName = BRIDGESFile.ReadLine().Trim();

          //Read extra lines:
          int nExtraLines = Convert.ToInt32(BRIDGESFile.ReadLine().Trim());
          var extras = new string[nExtraLines];
          for (var ii = 0; ii < nExtraLines; ++ii)
          {
            extras[ii] = BRIDGESFile.ReadLine();
          }

          //Create this group from data just read:
          var group = new BRIDGES();
          group.BridgeName = bridgeName;
          group.Parameters = parameters;
          group.SecondaryFileName = secondaryTableFileName;
          group.nExtraLines = nExtraLines;
          group.ExtraData = extras;

          //Store data in memory for this geometry group:
          BRIDGESFileContents.Add(group);

          //Store in mainTable a  row to be shown in bridges table in DIP tab: 
          // (bridge name, bridge geometry file name)
          var row = new string[2];
          row[0] = bridgeName;
          row[1] = secondaryTableFileName;





          mainTable.Add(row);

          //Potential new brige geometry group (secundary dependant table). Geometries (x, Bed, Z lower, Z upper, Deck) are 
          // stored in a file whose name could be repeated in the .BRIDGES file for another bridge. Only one copy of the file contents is 
          //stored in memory in BridgeGeometryGroups. If modified, only that copy is changed. 
          //It will be permanently stored when the user clicks "Save .BRIDGES".

          Universal.LoadSecondaryTable("BRIDGES", BRIDGESFilePath + "\\" + secondaryTableFileName,  ref warnings);

          //LoadBridgesSecondaryData(BRIDGESFilePath + "\\" + secondaryTableFileName, ref warnings);

        }
        catch (Exception ex)
        {
          MessageBox.Show(Universal.Idioma("ERROR 300417-1236 while proccessing  file:\n\n", "ERROR 300417-1236 procesando archivo:\n\n") + 
              BRIDGESFileName + "\n\n" + ex.Message,  "RiverFlow2D", MessageBoxButtons.OK,  MessageBoxIcon.Error);
        }
      }

      BRIDGESFile.Close();

      //Save variables associated to control tags:
      AddVariable("BRIDGES_VALUES", mainTable);
 
      //
      //
      //  Empty lines left intentionally

      if (warnings.Count > 0)
      {
        string warningsList = Universal.Idioma("WARNING 3004171237: The following bridges files do not exist: \n\n", "ADVERTENCIA 3004171237: Los siguientes archivos de puentes no existen: \n\n");
        for (int i = 0; i < warnings.Count; i++)
        {
          warningsList += "   ° " + warnings[i] + "\n";
        }
        warningsList += Universal.Idioma("\nDefault files were created.", "\nAchivos por defecto fueron creados.");
        MessageBox.Show(warningsList, "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }

	//Save BRIDGES data to file.
	public virtual void Save(string filename)
	{
      try
      {
        if (BRIDGESFileContents.Count > 0)
        {
          { //Write .BRIDGES file:
            TextWriter w = new StreamWriter(filename);
            //number of bridges:
            w.WriteLine(BRIDGESFileContents.Count);



            for (int i = 0; i < BRIDGESFileContents.Count; ++i)
            {
              //Bridge name, parameters, profiles file name, rows
              w.WriteLine(BRIDGESFileContents[i].BridgeName);
              w.WriteLine(BRIDGESFileContents[i].Parameters);
              w.WriteLine(BRIDGESFileContents[i].SecondaryFileName);
              w.WriteLine(BRIDGESFileContents[i].nExtraLines);
              for (int j = 0; j < BRIDGESFileContents[i].nExtraLines; ++j)
              {
                w.WriteLine(BRIDGESFileContents[i].ExtraData[j]);
              }
            }
            w.Close();
          }

          // Write secondary geometry data files.
          int nColumns = 5;
          Universal.SaveSecondaryTables("BRIDGES", nColumns);

          //foreach (Universal.SecondaryGroup aGroup in Universal.SecondaryGroups.Values)
          //{
          //  if (aGroup.Component == "BRIDGES")
          //  {
          //    TextWriter secondaryFile = new StreamWriter(aGroup.FileName);

          //    secondaryFile.WriteLine(aGroup.NPoints);
          //    for (int i = 0; i < aGroup.NPoints; i++)
          //    {
          //      secondaryFile.WriteLine(aGroup.Table[i].Col0 + "   " +
          //                         aGroup.Table[i].Col1 + "   " +
          //                         aGroup.Table[i].Col2 + "   " +
          //                         aGroup.Table[i].Col3 + "   " +
          //                         aGroup.Table[i].F4);
          //    }
          //    secondaryFile.Close();
          //  }
          //}
        }
        else
        {
          MessageBox.Show(Universal.Idioma("The bridge table is empty. It was not saved.", "La tabla del puente esta vacía. No fue almacenada."), 
              "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
      }
      catch
      {
        MessageBox.Show(Universal.Idioma("ERROR 0405171625: error while saving .BRIDGES file.", "ERROR 0405171625: error almacenando .BRIDGES."),
            "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
		}

    private static void LoadBridgesSecondaryData(string secondaryFileName, ref List<string> warnings)
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
            thisGroup.Component = "BRIDGES";
            thisGroup.FileName = secondaryFileName;
            thisGroup.NPoints = Convert.ToInt64(secondaryFile.ReadLine());
            var thisTable = new Universal.SecondaryTable[thisGroup.NPoints];

            //Read every line with 5 values: X, bed, z lower, z upper, deck
            for (int j = 0; j <= thisGroup.NPoints - 1; ++j)
            {
              string next = secondaryFile.ReadLine();
              string[] splt = next.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
              thisTable[j].Col0 = splt[0];
              thisTable[j].Col1 = splt[1];
              thisTable[j].Col2 = splt[2];
              thisTable[j].Col3 = splt[3];
              thisTable[j].Col4 = splt[4];
            }

            secondaryFile.Close();

            //Store bridge profile table:
            thisGroup.Table = thisTable;

            //Store this new group of bridge geometries:
            Universal.SecondaryGroups.Add(secondaryFileName, thisGroup);
          }
          catch (Exception ex)
          {
            MessageBox.Show(Universal.Idioma("ERROR 3004171244: error reading file ", "ERROR 3004171244: error leyendo archivo ") + 
                "\n\n" + secondaryFileName + "\n\n" + ex.Message, "RiverFlow2D", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
          }
        }
        else
        {
          //Bridge profile file doesn't exist in memory.
          Universal.CreateSecondaryDefaultFile("BRIDGES", secondaryFileName, 5);
          if (warnings.IndexOf(secondaryFileName) == -1)
          {
            warnings.Add(secondaryFileName);
          }
        }
      }
    }
  }
}

