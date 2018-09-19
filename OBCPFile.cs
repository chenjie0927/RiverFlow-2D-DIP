using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Drawing;


namespace Simulation_Options
{
	class OBCPFile : InputFile
	{
		public string OBCPDirectory;
		public string OBCPFilePathAndName;

		//public class PolyZone
		//{
		//	public string HistogramFileName;
		//	public int NPoints;
		//	public List<PointF> ZonePolyPoints;
		//};

		//public List<PolyZone> RainEvapZones = new List<PolyZone>();

		public OBCPFile()
		{
			var emptyList = new List<string[]>();
			AddVariable("OBCP_VALUES", emptyList);
			AddVariable("OBCP_NODES_VALUES", emptyList);
			AddVariable("OBCP_SERIES_VALUES", emptyList);
		}

		//Load data from file
		public virtual void Load(string OBCPFileName)
		{
			OBCPDirectory = Path.GetDirectoryName(OBCPFileName);
			OBCPFilePathAndName = OBCPFileName;
			
			//Load all data into the data manager:
			StreamReader OBCPFile = File.OpenText(OBCPFileName);

			try
			{
				string nGroups = OBCPFile.ReadLine().Trim();
				int numberOfGroups = Convert.ToInt32(nGroups);

				var groups = new List<string[]>();

				for (int i = 0; i < numberOfGroups; ++i)
				{
					OBCPFile.ReadLine().Trim();
					string openBoundaryType = OBCPFile.ReadLine().Trim();
					string fileName = OBCPFile.ReadLine().Trim();
					var row = new string[4];
					row[0] = Convert.ToString(i + 1).Trim();
					switch (openBoundaryType)
					{
						case "1":
							row[1] = "1";
							row[2] = Universal.Idioma( "Water Surface Elev. vs Time (BCType 1)", "Cota de Superficie del Agua vs Tiempo (BCtype 1)");
							break;

						case "5":
							row[1] = "5";
							row[2] = Universal.Idioma("Discharge and Water Surface Elev. vs Time (BCType 5)" , "Caudal y Cota de Superficie de Agua vs Tiempo (BCType 5)");
							break;

						case "6":
							row[1] = "6";
							row[2] = Universal.Idioma("Discharge vs Time (BCType 6)" , "Caudal vs Tiempo (BCType 6)");
							break;

						case "9":
							row[1] = "9";
							row[2] = Universal.Idioma("Rating Table (WSE vs Discharge) (BCType 9)" , "Rating Table(WSE vs Discharge)(BCType 9)");
							break;

						case "10":
							row[1] = "10";
							row[2] = Universal.Idioma("Free Inflow/Outflow (BCType 10)" , "Salida/Entrada Libres (BCType 10)");
							break;

						case "11":
							row[1] = "11";
							row[2] = Universal.Idioma("Free Outflow (BCType 11)" , "Salida Libre (BCType 11)");
							break;

						case "12":
							row[1] = "12";
							row[2] = Universal.Idioma("Uniform Flow Outflow (BCType 12)" , "Flujo de Salida Libre (BCType 12)");
							break;

						case "26":
							row[1] = "26";
							row[2] = Universal.Idioma("Water and Sediment Discharges vs Time (BCType 26)" , "Caudal de Agua y Sedimentos vs Tiempo (BCtype 26)");
							break;

						default:
							MessageBox.Show(Universal.Idioma("ERROR 2302160733 error reading .OBCP file. Unknown boundary type", "ERROR 2302160733 error leyendo archivo .OBCP file. Tipo de contorno desconocido"),
                                "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
							break;

					}

					//row[1] = openBoundaryType;
					row[3] = (fileName == "0" ? "(No File)" : fileName);
					groups.Add(row);

					int nNodes = Convert.ToInt16(OBCPFile.ReadLine());
					for (int j = 0; j < nNodes; ++j)
					{
						OBCPFile.ReadLine();
					}
				}

				//Fill boundary conditions groups table.
				AddVariable("OBCP_VALUES", groups);
			}
			catch (Exception ex)
			{
				MessageBox.Show(Universal.Idioma("ERROR 0102160854: error reading .OBCP file. ", "ERROR 0102160854: error leyendo archivo .OBCP. ") +
                    ex.Message, 
                    "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			OBCPFile.Close();

		}

		//Save data to file
		public virtual void Save(string LINFFile)
		{
			TextWriter w = new StreamWriter(LINFFile);
			//try
			//{
			//	var openBoundaries = (List<string[]>)GetVariable("OBCP_VALUES");
			//	var numberOfInfiltrationZones = (int)GetVariable("NUMBER_OF_RAINEVAP_ZONES");

			//	if (numberOfInfiltrationZones > 0)
			//	{
			//		//Number of polyline zones: 
			//		w.WriteLine(numberOfInfiltrationZones);
			//		for (int j = 0; j < numberOfInfiltrationZones; ++j)
			//		{
			//			//Infiltration model file name:
			//			w.WriteLine(RainEvapZones[j].HistogramFileName.Trim());
			//			//Number of points in polyline:
			//			w.WriteLine(RainEvapZones[j].NPoints);
			//			for (int k = 0; k < RainEvapZones[j].NPoints; k++)
			//				//Next point in polyline:
			//				w.WriteLine(RainEvapZones[j].ZonePolyPoints[k].X + "  " + RainEvapZones[j].ZonePolyPoints[k].Y);
			//		}
			//	}
			//	else
			//	{
			//		//saveAllwarnings += "{0} " + "The infiltration table is empty. It was not saved.";
			//		MessageBox.Show("The rainfall/evaporation table is empty. It was not saved.", "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Information);
			//	}
			//}
			//catch
			//{
			//	MessageBox.Show(Universal.Idioma("ERROR 2302151150: error trying to save .LRAIN file.", "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
			//}

			//w.Close();
		}

		public virtual void LoadOBCPNodes(int group)
		{

			StreamReader OBCPFile = File.OpenText(OBCPFilePathAndName);
			try
			{
				string nGroups = OBCPFile.ReadLine().Trim();
				int numberOfGroups = Convert.ToInt32(nGroups);

				var nodes = new List<string[]>();

				for (int i = 0; i < numberOfGroups; ++i)
				{
					if (i == group)
					{
						string dummy  = OBCPFile.ReadLine().Trim();
						string openBoundaryType = OBCPFile.ReadLine().Trim(); 
						string fileName = OBCPFile.ReadLine().Trim();
	
						int nNodes = Convert.ToInt16(OBCPFile.ReadLine().Trim());
						
						for (int j = 0; j < nNodes; ++j)
						{
							string node = OBCPFile.ReadLine().Trim();
							var row = new string[2];
							row[0] = Convert.ToString(j + 1);
							row[1] = node;
							nodes.Add(row);
						}

						AddVariable("OBCP_NODES_VALUES", nodes);
						break;
					}
					else
					{
						string dummy = OBCPFile.ReadLine();
						string type = OBCPFile.ReadLine();
						string fileName = OBCPFile.ReadLine();

						int nNodes = Convert.ToInt16(OBCPFile.ReadLine().Trim());
						for (int j = 0; j < nNodes; ++j)
						{
							OBCPFile.ReadLine();
						}
					}
				}

				
			}
			catch (Exception ex)
			{
				MessageBox.Show(Universal.Idioma("ERROR 0102161050: error trying to read .OBCP file. ", "ERROR 0102161050: error leyendo archivo .OBCP. ") + 
                    ex.Message, "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			OBCPFile.Close();
			
		}

		public virtual void LoadOBCSeries(int group)
		{

			StreamReader OBCPFile = File.OpenText(OBCPFilePathAndName);

			try
			{
				string nGroups = OBCPFile.ReadLine().Trim();
				int numberOfGroups = Convert.ToInt32(nGroups);

				for (int i = 0; i < numberOfGroups; ++i)
				{
					if (i == group)
					{

						var series = new List<string[]>();
						string dummy = OBCPFile.ReadLine().Trim();
						string openBoundaryType = OBCPFile.ReadLine().Trim();
						string fileName = OBCPFile.ReadLine().Trim();
						if (fileName != "0")
						{
							StreamReader OBCPSeriesFile = File.OpenText(OBCPDirectory + "\\" + fileName);
							int nValues = Convert.ToInt16(OBCPSeriesFile.ReadLine());
							for (int j = 0; j < nValues; ++j)
							{
								string nextLine = OBCPSeriesFile.ReadLine();
								//Convert line read to any number of separeted values, to 
								//later add to table as many colums as values in line:
								string[] split = nextLine.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
								var row = new string[split.Length+1];
								//row[0] = Convert.ToString(j + 1);

								for (int k = 1; k <= split.Length; k++)
								{
									row[k] = split[k - 1];
								}

								series.Add(row);
							}
							AddVariable("OBCP_SERIES_VALUES", series);
							OBCPSeriesFile.Close();
						}

						break;
					}
					else
					{
						string dummy = OBCPFile.ReadLine();
						string openBoundaryType = OBCPFile.ReadLine();
						string fileName = OBCPFile.ReadLine();

						int nNodes = Convert.ToInt16(OBCPFile.ReadLine().Trim());
						for (int j = 0; j < nNodes; ++j)
						{
							OBCPFile.ReadLine();
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(Universal.Idioma("ERROR 1202161005: error trying to read .OBCP file. ", "ERROR 1202161005: error leyendo archivo .OBCP. ") + 
                    ex.Message, "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				OBCPFile.Close();
			}
		}
	}
}
