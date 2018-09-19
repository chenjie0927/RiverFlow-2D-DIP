using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;

namespace Simulation_Options
{
	class OILPLUSFile : InputFile
	{
		public string OILPLUSDirectory;

		public OILPLUSFile()
		{
			var emptyViscocityDensityPlus = new List<string[]>();
			var emptyTemperaturePlus = new List<string[]>();

			AddVariable("OILSPILLONLAND_VISCOCITYDENSITY_VALUES", emptyViscocityDensityPlus);
			AddVariable("OILSPILLONLAND_TEMPERATURE_VALUES", emptyTemperaturePlus);
			AddVariable("OILSPILLONLAND_VISCOSITY", 0.005);
			AddVariable("OILSPILLONLAND_DENSITY", 900);
            AddVariable("OILSPILLONLAND_FRR", 0);
            AddVariable("OILSPILLONLAND_YS", 0);

			//Hard Wired:
				//AddVariable("OILSPILLONLAND_FORMULA", 3); //Full Simplified Bingham.
				//AddVariable("OILSPILLONLAND_YIELDSTRESS", 0);
				//AddVariable("OILSPILLONLAND_THETA", 0);

		}

		//Load data from file .OILP
		public virtual void Load(string oilplusFileName)
		{
			OILPLUSDirectory = Path.GetDirectoryName(oilplusFileName);
			string temperaturesFileName = null;
			string viscosityDensityFileName = null;

			AddVariable("OILP_USE_CONSTANTS", 1);
			AddVariable("OILP_USE_TABLES", 0);

			//Read .OILP file.
			StreamReader oilplusFile = null;
			try
			{
				oilplusFile = File.OpenText(oilplusFileName);

				var nextLine = oilplusFile.ReadLine();

				var formula = Convert.ToInt32(nextLine);
                if (formula == 2)
                    formula = 1;
                else if (formula == 3)
                    formula = 2;
                else if (formula == 7)
                     formula = 3;
                else
                    formula = 1;

				nextLine = oilplusFile.ReadLine();
				var yieldStress = Convert.ToDouble(nextLine); //Hard wired.

				nextLine = oilplusFile.ReadLine();
				var viscosity = Convert.ToDouble(nextLine);

				nextLine = oilplusFile.ReadLine();
				var theta = Convert.ToDouble(nextLine);//Hard wired.

				nextLine = oilplusFile.ReadLine();
				var density = Convert.ToDouble(nextLine);

				if (oilplusFile.EndOfStream)
				{
					temperaturesFileName = "";
					viscosityDensityFileName = "";
				}
				else
				{
					temperaturesFileName = oilplusFile.ReadLine();
					if (oilplusFile.EndOfStream)
						viscosityDensityFileName = "";
					else
						viscosityDensityFileName = oilplusFile.ReadLine();
				}

				AddVariable("OILSPILLONLAND_VISCOSITY", viscosity);
				AddVariable("OILSPILLONLAND_DENSITY", density);
                AddVariable("OILSPILLONLAND_FRR", formula);
                AddVariable("OILSPILLONLAND_YS", yieldStress);

				temperaturesFileName = temperaturesFileName.Trim();
				viscosityDensityFileName = viscosityDensityFileName.Trim();
                
                if (temperaturesFileName!= "")
				{
					temperaturesFileName = OILPLUSDirectory + "\\" + temperaturesFileName;
					AddVariable("OILP_USE_CONSTANTS", 0);
					AddVariable("OILP_USE_TABLES", 1);
				}

				if (viscosityDensityFileName != "")
				{
					viscosityDensityFileName = OILPLUSDirectory + "\\" + viscosityDensityFileName;
					AddVariable("OILP_USE_CONSTANTS", 0);
					AddVariable("OILP_USE_TABLES", 1);
				}

				AddVariable("OILSPILLONLAND_TEMPERATURE_FILE", temperaturesFileName);
				AddVariable("OILSPILLONLAND_VISCOSITY_FILE", viscosityDensityFileName);


			}
			catch (Exception ex)
			{
				MessageBox.Show(Universal.Idioma("ERROR 1903151446: error while reading file " + oilplusFileName + ". " + Environment.NewLine + ex.Message,
                                                "ERROR 1903151446: errro leyendo archivo " + oilplusFileName + ". " + Environment.NewLine + ex.Message),
                    "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				if (oilplusFile != null)
				{
					oilplusFile.Close();
				}
			}
			//End Read .OILP file

			var emptyViscocityDensityPlus = new List<string[]>();
			var emptyTemperaturePlus = new List<string[]>();

			AddVariable("OILSPILLONLAND_VISCOCITYDENSITY_VALUES", emptyViscocityDensityPlus);
			AddVariable("OILSPILLONLAND_TEMPERATURE_VALUES", emptyTemperaturePlus);

			//Read temperatures file.
			if (temperaturesFileName != "")
			{
				var temperaturesPlus = new List<string[]>();
				if (ReadOilpTemperaturesFile(temperaturesFileName, temperaturesPlus))
					AddVariable("OILSPILLONLAND_TEMPERATURE_VALUES", temperaturesPlus);
			}

			//Read viscosity-density file.
			if (viscosityDensityFileName != "")
			{
				var viscosityDensityPlus = new List<string[]>();
				if (ReadOilpViscosityDensitiesFile(viscosityDensityFileName, viscosityDensityPlus))
					AddVariable("OILSPILLONLAND_VISCOCITYDENSITY_VALUES", viscosityDensityPlus);
			}
		}

		//Save data to file
		public virtual void Save(string OILPLUSFileName)
		{
			TextWriter w = new StreamWriter(OILPLUSFileName);
			try
			{
                var formula = (int) GetVariable("OILSPILLONLAND_FRR");


                if (formula == 1)
                {
                    w.WriteLine("2");
                }
                else if (formula == 2)
                {
                    w.WriteLine("3");
                }   
                else if (formula == 3)
                {
                    w.WriteLine("7");
                }
                else
                    w.WriteLine("99");

                var str = GetVariable("OILSPILLONLAND_YS");
                w.WriteLine(str);
                str =GetVariable("OILSPILLONLAND_VISCOSITY");
                w.WriteLine(str);
				w.WriteLine("0");    //Internal friction angle. Hard wired.
				str =GetVariable("OILSPILLONLAND_DENSITY");
				w.WriteLine(str);

				int usetables = (int) GetVariable("OILP_USE_TABLES");

				if (usetables == 1)
				{
					int index;

					//Temperatures file name:
					var temperaturesFileName = (string) GetVariable("OILSPILLONLAND_TEMPERATURE_FILE");
					if (temperaturesFileName != "")
					{
						index = temperaturesFileName.LastIndexOf("\\", System.StringComparison.Ordinal);
						temperaturesFileName = temperaturesFileName.Remove(0, index + 1);
					}
					else
						temperaturesFileName = " ";

					w.WriteLine(temperaturesFileName); //may write an empty line

					//Viscosities-densities filename file name:
					var viscosityDensityFileName = (string) GetVariable("OILSPILLONLAND_VISCOSITY_FILE");
					if (viscosityDensityFileName != "")
					{
						index = viscosityDensityFileName.LastIndexOf("\\", System.StringComparison.Ordinal);
						viscosityDensityFileName = viscosityDensityFileName.Remove(0, index + 1);
					}
					else
						viscosityDensityFileName = " ";
					
					w.WriteLine(viscosityDensityFileName); //may write an empty line

				}
			}
			catch
			{
				MessageBox.Show(Universal.Idioma("ERROR 2303150942: error trying to save .OILP file.", "ERROR 2303150942: error almacenando .OILP."),
                    "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			w.Close();
		}

		public static bool ReadOilpTemperaturesFile(string oilpTemperaturesFileName, List<string[]> temperaturesList)
		{

			StreamReader oilplusFileTemperatures = null;
			try
			{
				oilplusFileTemperatures = File.OpenText(oilpTemperaturesFileName);
				var nextLine = oilplusFileTemperatures.ReadLine();
				int nTimes = Convert.ToInt32(nextLine);
				for (int i = 0; i < nTimes; ++i)
				{

					nextLine = oilplusFileTemperatures.ReadLine();
					if (nextLine != null)
					{
						string[] split = nextLine.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

						var row = new string[2];
						row[0] = split[0];
						row[1] = split[1];
						temperaturesList.Add(row);
					}
				}
				return true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(Universal.Idioma("ERROR 2701160900: error while reading file " + oilpTemperaturesFileName + ". " + Environment.NewLine + ex.Message,
                                                "ERROR 2701160900: error leyendo archivo " + oilpTemperaturesFileName + ". " + Environment.NewLine + ex.Message),
                    "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			finally
			{
				if (oilplusFileTemperatures != null)
				{
					oilplusFileTemperatures.Close();
				}
			}
		}

		public static bool ReadOilpViscosityDensitiesFile(string oilpViscosityDensityFileName, List<string[]> viscosityDensityList)
		{
			StreamReader oilplusFileViscosityDensity = null;
			try
			{
				oilplusFileViscosityDensity = File.OpenText(oilpViscosityDensityFileName);
				var nextLine = oilplusFileViscosityDensity.ReadLine();
				int nTimes = Convert.ToInt32(nextLine);
				for (int i = 0; i < nTimes; ++i)
				{

					nextLine = oilplusFileViscosityDensity.ReadLine();
					if (nextLine != null)
					{
						string[] split = nextLine.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);

						var row = new string[3];
						row[0] = split[0];
						row[1] = split[1];
						row[2] = split[2];
						viscosityDensityList.Add(row);
					}
				}
				return true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(Universal.Idioma("ERROR 1903151533: error while reading file " + oilpViscosityDensityFileName + ". " + Environment.NewLine + ex.Message,
                                                 "ERROR 1903151533: error leyendo archivo " + oilpViscosityDensityFileName + ". " + Environment.NewLine + ex.Message),
                    "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			finally
			{
				if (oilplusFileViscosityDensity != null)
				{
					oilplusFileViscosityDensity.Close();
				}
			}
		}


		public static bool WriteOilpTemperaturesFile(string oilpTemperaturesFileName, List<string[]> temperaturesList)
		{

			StreamReader oilplusFileTemperatures = null;
			try
			{
				oilplusFileTemperatures = File.OpenText(oilpTemperaturesFileName);
				var nextLine = oilplusFileTemperatures.ReadLine();
				int nTimes = Convert.ToInt32(nextLine);
				for (int i = 0; i < nTimes; ++i)
				{

					nextLine = oilplusFileTemperatures.ReadLine();
					if (nextLine != null)
					{
						string[] split = nextLine.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

						var row = new string[2];
						row[0] = split[0];
						row[1] = split[1];
						temperaturesList.Add(row);
					}
				}
				return true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(Universal.Idioma("ERROR 3001162109: error while writing file " + oilpTemperaturesFileName + ". " + Environment.NewLine + ex.Message,
                                                "ERROR 3001162109: error leyendo archivo " + oilpTemperaturesFileName + ". " + Environment.NewLine + ex.Message),
                    "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			finally
			{
				if (oilplusFileTemperatures != null)
				{
					oilplusFileTemperatures.Close();
				}
			}
		}


	}
}


