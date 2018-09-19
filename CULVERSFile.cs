using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Simulation_Options
{
  class CULVERTSFile : InputFile
  {
    public CULVERTSFile()
    {
      var emptyCULVERTS = new List<string[]>();
      AddVariable("CULVERT_VALUES", emptyCULVERTS);
      AddVariable("NUMBER_OF_CULVERTS", 0);
    }

    // Load data from file.
    public virtual void Load(string CULVERTSFileName)
    {
      StreamReader CULVERSFile = File.OpenText(CULVERTSFileName);
      var warnings = new List<string>();

      int index = CULVERTSFileName.LastIndexOf("\\", StringComparison.Ordinal);
      string CULVERTSFilePath = CULVERTSFileName.Remove(index);

      //Numberof culvert groups in .CULVERTS file:
      string nGroups = CULVERSFile.ReadLine();
      int numberOfGroups = Convert.ToInt32(nGroups);
      AddVariable("NUMBER_OF_CULVERTS", numberOfGroups);

      //Reset data structures to store .CULVERTS in local memory:
      var mainTable = new List<string[]>();
      Universal.CulvertValuesGroups.Clear();

      //
      //(Code space for individual variables)
      //

      for (int i = 0; i < numberOfGroups; ++i)
      {
        try
        {
          // Culvert name and type.
          string secondaryTableFileName = CULVERSFile.ReadLine().Trim();
          string culvertType = CULVERSFile.ReadLine().Trim();

          var row = new string[7];

          row[0] = secondaryTableFileName.Trim();
          switch (Convert.ToInt32(culvertType))
          {
            case 0:
              row[1] = "rating table";
              break;

            case 1:
              row[1] = "box";
              break;

            case 2:
              row[1] = "circular";
              break;

            default:
              row[1] = "?";
              break;
          }

          // Culvert file.
          string culvertsFileName = CULVERSFile.ReadLine().Trim();
          row[2] = culvertsFileName;

          string line = CULVERSFile.ReadLine();
          string[] split = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

          //x1 y1
          row[3] = split[0];
          row[4] = split[1];

          //x2 y2
          row[5] = split[2];
          row[6] = split[3];

          mainTable.Add(row);

          //Potential new culverts data group (secundary dependant data). It could be a table (Depth, Discharge) or
          //scatered data, they are stored in a file whose name could be repeated in the .CULVERTS file for another 
          //culvert. Only one copy of the file contents is stored in memory in CulvertValuesGroup structure. 
          //If modified, only that copy is changed. It is permanently stored when
          //the user clicks "Save .CULVERT".

          //Universal.LoadSecondaryTable("CULVERTS", CULVERTSFileName + "\\" + secondaryTableFileName, 2, ref warnings);

          LoadCulvertsSecondaryData(CULVERTSFilePath + "\\" + culvertsFileName, ref culvertType, ref warnings);


        }
        catch (Exception ex)
        {
          MessageBox.Show(Universal.Idioma("ERROR 0805170838: error while proccessing  ", "ERROR 0805170838: error procesando  ") + CULVERTSFileName + ". " +
                            Environment.NewLine + ex.Message, "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }

      CULVERSFile.Close();

      AddVariable("CULVERT_VALUES", mainTable);

      if (warnings.Count > 0)
      {
        string warningsList = Universal.Idioma("WARNING 0811121033: The following culvert files do not exist: \n\n",
                                                        "ADVERTENCIA 0811121033: Los sigientes archivos de alcantarillas no existen: \n\n");
        for (int i = 0; i < warnings.Count; i++)
        {
          warningsList += "   ° " + warnings[i] + "\n";

        }
        warningsList += Universal.Idioma("\nDefault files were created.","\nAchivos por defecto fueron creados.");
        MessageBox.Show(warningsList, "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }

    //Save data to file
    public virtual void Save(string filename)
    {
      try
      {
        var culverts = (List<string[]>)GetVariable("CULVERT_VALUES");

        var numberOfRows = (int)GetVariable("NUMBER_OF_CULVERTS");
        if (numberOfRows > 0)
        {
          TextWriter w = new StreamWriter(filename);
          //number of culverts
          w.WriteLine(culverts.Count);
          for (int i = 0; i < culverts.Count; ++i)
          {
            //culvert name
            w.WriteLine(culverts[i][0]);
            //culvert type
            switch (culverts[i][1].Trim())
            {
              case "rating table":
                w.WriteLine("0");
                break;

              case "box":
                w.WriteLine("1");
                break;

              case "circular":
                w.WriteLine("2");
                break;

              default:
                w.WriteLine("99");
                break;
            }

            //culvert file
            w.WriteLine(culverts[i][2]);
            //X1,Y1,X2,Y2
            w.WriteLine(culverts[i][3] + " " + culverts[i][4] + " " + culverts[i][5] + " " + culverts[i][6]);
          }
          w.Close();

          // Write culvert data files.
          foreach (Universal.CulvertValuesGroup aCulvert in Universal.CulvertValuesGroups.Values)
          {
            TextWriter culvertFile = new StreamWriter(aCulvert.FileName);
            switch (aCulvert.Type)
            {
              case 0: // rating table
                culvertFile.WriteLine(aCulvert.NPoints);
                for (int i = 0; i < aCulvert.NPoints; i++)
                {
                  culvertFile.WriteLine(aCulvert.Table[i].Col0 + "   " + aCulvert.Table[i].Col1);
                }
                culvertFile.WriteLine(aCulvert.Z1);
                culvertFile.WriteLine(aCulvert.Z2);
                culvertFile.WriteLine(aCulvert.UseElementElevations);
                break;

              case 1: // box
                culvertFile.WriteLine(aCulvert.Nb);
                culvertFile.WriteLine(aCulvert.Ke);
                culvertFile.WriteLine(aCulvert.nc);
                culvertFile.WriteLine(aCulvert.Kp);
                culvertFile.WriteLine(aCulvert.M);
                culvertFile.WriteLine(aCulvert.cp);
                culvertFile.WriteLine(aCulvert.Y);
                culvertFile.WriteLine(aCulvert.m);
                culvertFile.WriteLine(aCulvert.Hb);
                culvertFile.WriteLine(aCulvert.Base);
                culvertFile.WriteLine(aCulvert.Z1);
                culvertFile.WriteLine(aCulvert.Z2);
                culvertFile.WriteLine(aCulvert.ComboManningSelectionIndex);
                culvertFile.WriteLine(aCulvert.ComboEntranceLossSelectionIndex);
                culvertFile.WriteLine(aCulvert.ComboInletCoeffsSelectionIndex);
                culvertFile.WriteLine(aCulvert.UseElementElevations);
                break;

              case 2: //circular
                culvertFile.WriteLine(aCulvert.Nb);
                culvertFile.WriteLine(aCulvert.Ke);
                culvertFile.WriteLine(aCulvert.nc);
                culvertFile.WriteLine(aCulvert.Kp);
                culvertFile.WriteLine(aCulvert.M);
                culvertFile.WriteLine(aCulvert.cp);
                culvertFile.WriteLine(aCulvert.Y);
                culvertFile.WriteLine(aCulvert.m);
                culvertFile.WriteLine(aCulvert.Dc);
                culvertFile.WriteLine(aCulvert.Z1);
                culvertFile.WriteLine(aCulvert.Z2);
                culvertFile.WriteLine(aCulvert.ComboManningSelectionIndex);
                culvertFile.WriteLine(aCulvert.ComboEntranceLossSelectionIndex);
                culvertFile.WriteLine(aCulvert.ComboInletCoeffsSelectionIndex);
                culvertFile.WriteLine(aCulvert.UseElementElevations);
                break;
            }
            culvertFile.Close();
          }
        }
        else
        {
          MessageBox.Show(Universal.Idioma("The culverts table is empty. It was not saved.", "La tabla de alcantarillas está vacía. No fue almacenada."), 
              "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
      }
      catch
      {
        MessageBox.Show(Universal.Idioma("ERROR 1199111023: error while saving .CULVERTS file.", "ERROR 1199111023: error almacenando archivo .CULVERTS."),
             "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    // Read file names.
    public string[] ReadFileNames(string filename)
    {
      StreamReader s = File.OpenText(filename);
      string read = s.ReadLine();
      int numberOfCoulverts = Convert.ToInt32(read);

      var fileNames = new string[numberOfCoulverts];

      try
      {
        for (int i = 0; i < numberOfCoulverts; ++i)
        {
          //culvert ID
          s.ReadLine();
          //culvert type
          s.ReadLine();
          //name of file
          fileNames[i] = s.ReadLine();
          //x1 y1 x2 y2
          s.ReadLine();
        }
      }
      catch
      {
        MessageBox.Show(Universal.Idioma("ERROR 1412110914: error trying to read .CULVERTS file.", "ERROR 1412110914: error leyendo archivo .CULVERTS."),
            "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      s.Close();
      return fileNames;
    }


    private static void LoadCulvertsSecondaryData(string wholeFileName, ref string culvertType,ref List<string> warnings)
    {
      if (!Universal.CulvertValuesGroups.ContainsKey(wholeFileName))
      {
        if (File.Exists(wholeFileName))
        {
          try
          {
            var data = new Universal.CulvertValuesGroup();
            StreamReader culvertFile;
            culvertFile = File.OpenText(wholeFileName);
            switch (Convert.ToInt32(culvertType)) //culvert type
            {
              case 0: //rating table
                data.Type = 0;
                data.FileName = wholeFileName;
                data.NPoints = Convert.ToInt64(culvertFile.ReadLine());
                var myRt = new Universal.SecondaryTable[data.NPoints];

                for (int j = 0; j <= data.NPoints - 1; ++j)
                {

                  string next = culvertFile.ReadLine();
                  string[] split = next.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                  if (split.Length == 2)
                  {
                    myRt[j].Col0 = split[0];
                    myRt[j].Col1 = split[1];
                  }
                  else if (split.Length == 1)
                  {
                    myRt[j].Col0 = split[0];
                    myRt[j].Col1 = "";
                  }
                  else
                  {
                    myRt[j].Col0 = "";
                    myRt[j].Col1 = "";
                  }

                  //Universal.CulvertValuesGroups.V
                }
                data.Z1 = culvertFile.ReadLine();
                data.Z2 = culvertFile.ReadLine();
                data.UseElementElevations = Convert.ToInt32(culvertFile.ReadLine());

                culvertFile.Close();
                data.Table = myRt;
                Universal.CulvertValuesGroups.Add(wholeFileName, data);
                break;

              case 1: // Variables type 1 (box)
                data.Type = 1;
                data.Nb = culvertFile.ReadLine();
                data.FileName = wholeFileName;
                data.Ke = culvertFile.ReadLine();
                data.nc = culvertFile.ReadLine();
                data.Kp = culvertFile.ReadLine();
                data.M = culvertFile.ReadLine();
                data.cp = culvertFile.ReadLine();
                data.Y = culvertFile.ReadLine();
                data.m = culvertFile.ReadLine();
                data.Hb = culvertFile.ReadLine();
                data.Base = culvertFile.ReadLine();
                data.Z1 = culvertFile.ReadLine();
                data.Z2 = culvertFile.ReadLine();
                data.ComboManningSelectionIndex = Convert.ToInt32(culvertFile.ReadLine());
                data.ComboEntranceLossSelectionIndex = Convert.ToInt32(culvertFile.ReadLine());
                data.ComboInletCoeffsSelectionIndex = Convert.ToInt32(culvertFile.ReadLine());
                data.UseElementElevations = Convert.ToInt32(culvertFile.ReadLine());

                culvertFile.Close();
                Universal.CulvertValuesGroups.Add(wholeFileName, data);
                break;

              case 2: // Variables type 2 (circular)
                data.Type = 2;
                data.Nb = culvertFile.ReadLine();
                data.FileName = wholeFileName;
                data.Ke = culvertFile.ReadLine();
                data.nc = culvertFile.ReadLine();
                data.Kp = culvertFile.ReadLine();
                data.M = culvertFile.ReadLine();
                data.cp = culvertFile.ReadLine();
                data.Y = culvertFile.ReadLine();
                data.m = culvertFile.ReadLine();
                data.Dc = culvertFile.ReadLine();
                data.Z1 = culvertFile.ReadLine();
                data.Z2 = culvertFile.ReadLine();
                data.ComboManningSelectionIndex = Convert.ToInt32(culvertFile.ReadLine());
                data.ComboEntranceLossSelectionIndex = Convert.ToInt32(culvertFile.ReadLine());
                data.ComboInletCoeffsSelectionIndex = Convert.ToInt32(culvertFile.ReadLine());
                data.UseElementElevations = Convert.ToInt32(culvertFile.ReadLine());

                culvertFile.Close();
                Universal.CulvertValuesGroups.Add(wholeFileName, data);
                break;
            }
          }
          catch (Exception ex)
          {
            MessageBox.Show(Universal.Idioma("ERROR 1303130759: error reading file ", "ERROR 1303130759: error leyendo archivo ") + 
                wholeFileName + ". " + ex.Message, "RiverFlow2D", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
          }
        }
        else
        {
          // Culvert file doesn't exist. See if a default file was created.
          if (warnings.IndexOf(wholeFileName) == -1)
          {
            // Write this default file.
            warnings.Add(wholeFileName);

            if (!(culvertType == "0" | culvertType == "1" | culvertType == "2"))
              culvertType = "1";

            CreateCulvertDefaultFile(wholeFileName, culvertType);
          }

        }
      }
    }

    private static void CreateCulvertDefaultFile(string culvertFileName, string culvertType)
    {
      var data = new Universal.CulvertValuesGroup();
      TextWriter culvertFile = new StreamWriter(culvertFileName);
      switch (Convert.ToInt32(culvertType))
      {
        case 0: //rating table
          data.Type = 0;
          data.FileName = culvertFileName;
          data.NPoints = 1;
          var myRt = new Universal.SecondaryTable[data.NPoints];
          myRt[0].Col0 = "0";
          myRt[0].Col1 = "0";
          data.Table = myRt;
          data.Z1 = "0.0";
          data.Z1 = "0.0";
          data.UseElementElevations = 1;
          Universal.CulvertValuesGroups.Add(culvertFileName, data);

          // Write default data.
          culvertFile.WriteLine(data.NPoints);
          culvertFile.WriteLine(data.Table[0].Col0 + "   " + data.Table[0].Col1);
          culvertFile.WriteLine(data.Z1);
          culvertFile.WriteLine(data.Z2);
          culvertFile.WriteLine(data.UseElementElevations);
          break;

        case 1: // Variables type 1 (box)
          data.Type = 1;
          data.Nb = "1";
          data.FileName = culvertFileName;
          data.Ke = "0.5";
          data.nc = "0.025";
          data.Kp = "1.5";
          data.M = "0.5";
          data.cp = "1.0";
          data.Y = "0.8";
          data.m = "0.7";
          data.Hb = "1.0";
          data.Base = "1.0";
          data.Z1 = "0.0";
          data.Z1 = "0.0";
          data.ComboManningSelectionIndex = 0;
          data.ComboEntranceLossSelectionIndex = 0;
          data.ComboInletCoeffsSelectionIndex = 0;
          data.UseElementElevations = 1;
          Universal.CulvertValuesGroups.Add(culvertFileName, data);

          // Write default data.
          culvertFile.WriteLine(data.Nb);
          culvertFile.WriteLine(data.Ke);
          culvertFile.WriteLine(data.nc);
          culvertFile.WriteLine(data.Kp);
          culvertFile.WriteLine(data.M);
          culvertFile.WriteLine(data.cp);
          culvertFile.WriteLine(data.Y);
          culvertFile.WriteLine(data.m);
          culvertFile.WriteLine(data.Hb);
          culvertFile.WriteLine(data.Base);
          culvertFile.WriteLine(data.Z1);
          culvertFile.WriteLine(data.Z2);
          culvertFile.WriteLine(data.ComboManningSelectionIndex);
          culvertFile.WriteLine(data.ComboEntranceLossSelectionIndex);
          culvertFile.WriteLine(data.ComboInletCoeffsSelectionIndex);
          culvertFile.WriteLine(data.UseElementElevations);

          data.UseElementElevations = 1;

          break;

        case 2: // Variables type 2 (circular)
          data.Type = 2;
          data.Nb = "1";
          data.FileName = culvertFileName;
          data.Ke = "0.5";
          data.nc = "0.025";
          data.Kp = "1.5";
          data.M = "0.5";
          data.cp = "1.0";
          data.Y = "0.8";
          data.m = "0.7";
          data.Dc = "1";
          data.Z1 = "0.0";
          data.Z1 = "0.0";
          data.ComboManningSelectionIndex = 0;
          data.ComboEntranceLossSelectionIndex = 0;
          data.ComboInletCoeffsSelectionIndex = 0;
          data.UseElementElevations = 1;
          Universal.CulvertValuesGroups.Add(culvertFileName, data);

          // Write default data.
          culvertFile.WriteLine(data.Nb);
          culvertFile.WriteLine(data.Ke);
          culvertFile.WriteLine(data.nc);
          culvertFile.WriteLine(data.Kp);
          culvertFile.WriteLine(data.M);
          culvertFile.WriteLine(data.cp);
          culvertFile.WriteLine(data.Y);
          culvertFile.WriteLine(data.m);
          culvertFile.WriteLine(data.Dc);
          culvertFile.WriteLine(data.Z1);
          culvertFile.WriteLine(data.Z2);
          culvertFile.WriteLine(data.ComboManningSelectionIndex);
          culvertFile.WriteLine(data.ComboEntranceLossSelectionIndex);
          culvertFile.WriteLine(data.ComboInletCoeffsSelectionIndex);
          culvertFile.WriteLine(data.UseElementElevations);
          break;
      }
      culvertFile.Close();
    }
  }
}
