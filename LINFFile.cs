using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace Simulation_Options
{
	class LINFFile : InputFile
	{
    public LINFFile()
		{
			var emptyInfiltrations = new List<string[]>();
			AddVariable("INFILTRATION_VALUES", emptyInfiltrations);
			AddVariable("NUMBER_OF_INFILTRATION_ZONES", 0);
		}

    public class LINF  //Stores a copy of the .GATES file
    {
      public string InfiltrationParametersFileName;
      public int NZoneVertices;
      public string[] ZonePolygon;
    }

    public List<LINF> WholeLINFFileContents = new List<LINF>();

		//Load data from file
		public virtual void Load(string LINFFileName)
		{
			StreamReader LINFFile = File.OpenText(LINFFileName);
      var warnings = new List<string>();
      int index = LINFFileName.LastIndexOf("\\", StringComparison.Ordinal);
      string LINFFilePath = LINFFileName.Remove(index);

      //Read number of infiltration groups in .LINF file:
      string nLINF = LINFFile.ReadLine();
      int numberOfInfiltrations = Convert.ToInt32(nLINF);
      AddVariable("NUMBER_OF_INFILTRATION_ZONES", numberOfInfiltrations);

      //Reset data structs:
          var infiltrationTable = new List<string[]>();  //Structure for the table shown in panel
          Universal.InfiltrationParametersFiles.Clear();
          WholeLINFFileContents.Clear();

      for (int i = 0; i < numberOfInfiltrations; ++i)
      { //Read all rest of data groups from .LINF file....
        try
        {
          //Parameters file name:
          string parametersFileName = LINFFile.ReadLine().Trim();

          //Read pairs of vertices of polygons for this infiltration zone, 
          //not to be shown but to restore them later.
          int nVertices = Convert.ToInt32(LINFFile.ReadLine().Trim());
          var polygonPoints = new string[nVertices];
          for (var ii = 0; ii < nVertices; ++ii)
          {
            polygonPoints[ii] = LINFFile.ReadLine();
          }

          //Create this polygon group from data just read:
          var currentPolygonGroup = new LINF();
          currentPolygonGroup.InfiltrationParametersFileName = parametersFileName;
          currentPolygonGroup.NZoneVertices = nVertices;
          currentPolygonGroup.ZonePolygon = polygonPoints;

          //Store data in memory for this polygon zone:
          WholeLINFFileContents.Add(currentPolygonGroup);

          //Store in infiltrationTable a  row to be shown in gates table in DIP tab: 
          // (gate name, crest elevation, gate height, Cd, openings table name)
          var row = new string[2];
          row[0] = (i+1).ToString();
          row[1] = parametersFileName;

          infiltrationTable.Add(row);

          //Potential new parameters group (secundary dependant data). Depending on the model type (Horton, Green-Ampt, SCS-CN)
          // stored in a file whose name could be repeated in the .LINF file for another zone. Only one copy of the file contents is 
          //stored in memory in OpeningsFiles (of GATEOpenings structure). If modified, only that copy is changed. 
          //It will be permanently stored when the user clicks "Save .GATES".

                LoadInfiltrationSecondaryData(LINFFilePath + "\\" + parametersFileName, ref warnings);

        }
        catch (Exception ex)
        {
          MessageBox.Show(Universal.Idioma("ERROR 2504172221: error while proccessing  ", "ERROR 2504172221: error procesando  ") +
              LINFFileName + ". " +
                            Environment.NewLine + ex.Message, "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }

      LINFFile.Close();

      //Save gatesTable data in "datagates" DataGridView control in the GATES panel.
      AddVariable("INFILTRATION_VALUES", infiltrationTable);

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


	//Save data to file
	public virtual void Save(string LINFFileName)
	{
      try
      {
        if (WholeLINFFileContents.Count > 0)
        {

          {//Write .LINF file:
            TextWriter infiltrationFile = new StreamWriter(LINFFileName);
            //number of innfiltration zones::
            infiltrationFile.WriteLine(WholeLINFFileContents.Count);

            for (int i = 0; i < WholeLINFFileContents.Count; ++i)
            {
              //parameters file name:
              infiltrationFile.WriteLine(WholeLINFFileContents[i].InfiltrationParametersFileName);
              //zone polygon corrdinates:
              infiltrationFile.WriteLine(WholeLINFFileContents[i].NZoneVertices);
              for (int j = 0; j < WholeLINFFileContents[i].NZoneVertices; ++j)
              {
                infiltrationFile.WriteLine(WholeLINFFileContents[i].ZonePolygon[j]);
              }
            }
            infiltrationFile.Close();
          }

          {//Write infiltration parameters data files. Only one copy of each parameters file is kept in InfiltrationParameters.
            foreach (Universal.InfiltrationParametersGroup anInfiltrationGroup in Universal.InfiltrationParametersFiles.Values)
              {
                TextWriter parametersFile = new StreamWriter(anInfiltrationGroup.ParametersFileName);
                //model type:
                
                if (anInfiltrationGroup.ModelType == "1")  //Horton model
                {
                  parametersFile.WriteLine("1"); //model
                  parametersFile.WriteLine("3"); //number of parameters
                  parametersFile.WriteLine(anInfiltrationGroup.DecayRate + "  " +
                                           anInfiltrationGroup.FinalRate + "  " +
                                           anInfiltrationGroup.InitialRate);
                }

                else if (anInfiltrationGroup.ModelType == "2") //Green-Ampt model
                {
                  parametersFile.WriteLine("2");  //model
                  parametersFile.WriteLine("3");  //number of parameters
                  parametersFile.WriteLine(anInfiltrationGroup.DecayRate + "  " +
                                         anInfiltrationGroup.FinalRate + "  " +
                                         anInfiltrationGroup.InitialRate);
                //parametersFile.WriteLine(anInfiltrationGroup.HydraulicConductivity + "  " +
                //                          anInfiltrationGroup.PsiWetting + "  " +
                //                          anInfiltrationGroup.DeltaTheta);
                }

                else if (anInfiltrationGroup.ModelType == "3") //SCS-CN model
                {
                  parametersFile.WriteLine("3");  //model
                  parametersFile.WriteLine("4");  //number of 
                                parametersFile.WriteLine(anInfiltrationGroup.DecayRate + "  " +
                                                       anInfiltrationGroup.FinalRate + "  0  0");
                //parametersFile.WriteLine(anInfiltrationGroup.CNCurveNumber + "  " +
                //                          anInfiltrationGroup.InitialAbstraction);
                }

                parametersFile.Close();
              }
          }
        }
        else
        {
          MessageBox.Show(Universal.Idioma("The infiltration paremeters table is empty. It was not saved.", 
              "Ta tabla de parámetros de infiltración esta vacía. No fue almacenada"), 
              "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
      }
      catch
      {
        MessageBox.Show(Universal.Idioma("ERROR 2604170640: error while saving .LINF file.", "ERROR 2604170640: error almacenando .LINF."),
            "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }


      //TextWriter w = new StreamWriter(LINFFile);
      //try
      //{
      //	var infiltrations = (List<string[]>)GetVariable("INFILTRATION_VALUES");
      //	var numberOfInfiltrationZones = (int)GetVariable("NUMBER_OF_INFILTRATION_ZONES");

      //	if (numberOfInfiltrationZones > 0)
      //	{
      //		//Number of polyline zones: 
      //		int nZones = infiltrations.Count;
      //		w.WriteLine(nZones);
      //		for (int j = 0; j < nZones; ++j)
      //		{
      //			//Infiltration model file name:
      //			if (InfilZones[j].ModelFileName.Trim() != null)
      //			{
      //				w.WriteLine(InfilZones[j].ModelFileName.Trim());
      //				//Number of points in polyline:
      //				w.WriteLine(InfilZones[j].NPoints);
      //				//Polyline points:
      //				for (int k = 0; k < InfilZones[j].NPoints; k++)
      //					//Next point in polyline:
      //					w.WriteLine(InfilZones[j].ZonePolyPoints[k].X + "  " + InfilZones[j].ZonePolyPoints[k].Y);
      //			}
      //		}
      //	}
      //	else
      //	{
      //		//saveAllwarnings += "{0} " + "The infiltration table is empty. It was not saved.";
      //		MessageBox.Show("The infiltrations table is empty. It was not saved.", "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Information);
      //	}
      //}
      //catch
      //{
      //	MessageBox.Show(Universal.Idioma("ERROR 2504172140: error trying to save .LINF file.", "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
      //}

      //w.Close();
    }



    private static void LoadInfiltrationSecondaryData(string wholeFileName, ref List<string> warnings)
    {
      // Create new item in InfiltrationParametersFiles structure (of type InfiltrationParametersGroup) (if it is not already there) with 
      // data from infiltration parameters file name.

      if (!Universal.InfiltrationParametersFiles.ContainsKey(wholeFileName))
      {
        if (File.Exists(wholeFileName))
        {
          try
          {
            var thisInfiltrationParameters = new Universal.InfiltrationParametersGroup();
            StreamReader parametersFile;
            parametersFile = File.OpenText(wholeFileName);

            string modelType = parametersFile.ReadLine();
            string nParameters = parametersFile.ReadLine();
            string parameters = parametersFile.ReadLine();
            string[] split = parameters.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            parametersFile.Close();

            //Store parameters:
            thisInfiltrationParameters.ModelType = modelType;
            thisInfiltrationParameters.ParametersFileName = wholeFileName;
            switch (Convert.ToInt32(modelType))
            {
              case 1:  //Horton model
                thisInfiltrationParameters.DecayRate = split[0];
                thisInfiltrationParameters.FinalRate = split[1];
                thisInfiltrationParameters.InitialRate = split[2];
                break;

              case 2:  //HorGreen-Ampt model
                thisInfiltrationParameters.HydraulicConductivity = split[0];
                thisInfiltrationParameters.PsiWetting = split[1];
                thisInfiltrationParameters.DeltaTheta = split[2];

                thisInfiltrationParameters.DecayRate = split[0];
                thisInfiltrationParameters.FinalRate = split[1];
                thisInfiltrationParameters.InitialRate = split[2];

                break;

              case 3:  //SCS-CN model
                thisInfiltrationParameters.CNCurveNumber = split[0];
                thisInfiltrationParameters.InitialAbstraction = split[1];

                thisInfiltrationParameters.DecayRate = split[0];
                thisInfiltrationParameters.FinalRate = split[1];
                thisInfiltrationParameters.InitialRate = "";
                break;

            }

            //Store this new group of parameters:
            Universal.InfiltrationParametersFiles.Add(wholeFileName, thisInfiltrationParameters);
          }
          catch (Exception ex)
          {
            MessageBox.Show(Universal.Idioma("ERROR 2504172139: error reading file ", "ERROR 2504172139: error leyendo archivo ") + 
                wholeFileName + ". " + ex.Message, "RiverFlow2D", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
          }
        }
        else
        {
          //Gates apertures file doesn't exist. See if a default file was created.
          if (warnings.IndexOf(wholeFileName) == -1)
          {
            // Write this default file.
            warnings.Add(wholeFileName);
            CreateInfiltrationParametersDefaultFile(wholeFileName);
          }
        }

      }
    }

    private static void CreateInfiltrationParametersDefaultFile(string infiltrationParametersFileName)
    {
      //Add an empty parameters group to InfiltrationParameters:
      var anInfiltrationGroup = new Universal.InfiltrationParametersGroup();
      anInfiltrationGroup.ParametersFileName = infiltrationParametersFileName;
      anInfiltrationGroup.ModelType = "1";
      anInfiltrationGroup.DecayRate = "0";
      anInfiltrationGroup.FinalRate = "0";
      anInfiltrationGroup.InitialRate = "0";
      Universal.InfiltrationParametersFiles.Add(infiltrationParametersFileName, anInfiltrationGroup);

      // Write new empty infiltration parameters file:
      TextWriter infiltrationParametersFile = new StreamWriter(infiltrationParametersFileName);
      infiltrationParametersFile.WriteLine("1");
      infiltrationParametersFile.WriteLine("0  0  0");
      infiltrationParametersFile.Close();
    }

  }
}

