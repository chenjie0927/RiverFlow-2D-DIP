using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace Simulation_Options
{
	class SEDPLUSFile : ConfiguratedFile
	{
		public SEDPLUSFile()
		{		
			var empty = new List<string[]>();
			//Suspended sediment.
			AddVariable("SUSPENDEDSEDIMENT_VALUES", empty);
			AddVariable("SUSPENDED_SEDIMENT_NUMBER_OF_SEDIMENTS", 0);
			AddVariable("SUSPENDED_SEDIMENT_TYPE_FORMULA", 0);
			AddVariable("SUSPENDED_SEDIMENT_TYPE_WS", 0);
			AddVariable("SUSPENDED_SEDIMENT_FACTOR_WS", 0);
			AddVariable("SUSPENDED_SEDIMENT_FACTOR", 0);
            AddVariable("SUSPENDED_SEDIMENT_LONGITUDINAL_DISP_COEFF", 0);
            AddVariable("SUSPENDED_SEDIMENT_TRANSVERSAL_DISP_COEFF", 0);

            //Bed Load sediment.
            AddVariable("BEDLOADSEDIMENT_VALUES", empty);
			AddVariable("BED_LOAD_SEDIMENT_NUMBER_OF_SEDIMENTS",0);
			AddVariable("BED_LOAD_SEDIMENT_TYPE_FORMULA", 0);
            AddVariable("BED_LOAD_COUPLED_COMPUTATION",1);

			//Mode.
			AddVariable("SUSPENDED_SEDIMENT_PLUS_ACTIVE", 1);
			AddVariable("BED_LOAD_SEDIMENT_PLUS_ACTIVE", 1);

			//Get the .SEDPLUS configuration filename
			StreamReader s = File.OpenText(Universal.ConfigConfig);
			string read = s.ReadLine();
			while (read.Substring(0, 7) != "SEDPLUS") read = s.ReadLine();
			s.Close();

			//Configurate the .SEDPLUS filename
			CreateConfig(read.Substring(8));			
		
		}

		public static bool SEDSLoaded;
		public static bool SEDBLoaded;

		//Load data from file
		public virtual void Load(string filename)
		{
			//Load all data into the data manager.

			LoadSEDSFile(filename);
			LoadSEDBFile(filename);
		}

		private void LoadSEDSFile(string fileName)
		{

			if (! File.Exists(fileName))
			{
				MessageBox.Show(Universal.Idioma("File " + fileName + " doesn't exist.", "File " + fileName + " doesn't exist."), 
                    "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
				SEDSLoaded = false;
				return;
			}

			var fileinfo = new FileInfo(fileName);
			if (fileinfo.Length < 1)
			{
				MessageBox.Show(Universal.Idioma("File " + fileName + " is empty.", "File " + fileName + " está vacío."),
                    "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
				SEDSLoaded = false;
				return ;
			}

			string units;
			if (Universal.RiverUnits == "Metric") units = "(mts)";
			else units = "(ft)";

			StreamReader s = File.OpenText(fileName);
			try
			{
                //string txt;
				string read = s.ReadLine();
				int activeSuspendedSediment = Convert.ToInt32(read);

				read = s.ReadLine();
				int numberOfSediments = Convert.ToInt32(read);

				read = s.ReadLine();
				int typeFormula = Convert.ToInt32(read);

				var suspendedSedimentData = new List<string[]>();

				string density = s.ReadLine();                  // Line contains n values, one value for each concentration (see concentrations table in DIP).
				string initialConcentration = s.ReadLine();     // Line contains n values, one value for each concentration (see concentrations table in DIP).
                string diameter30 = s.ReadLine();               // Line contains n values, one value for each concentration (see concentrations table in DIP).
                string diameter50 = s.ReadLine();               // Line contains n values, one value for each concentration (see concentrations table in DIP).
                string diameter90 = s.ReadLine();               // Line contains n values, one value for each concentration (see concentrations table in DIP).
                string porosity = s.ReadLine();                 // Line contains n values, one value for each concentration (see concentrations table in DIP).
                string criticalShieldStress = s.ReadLine();     // Line contains n values, one value for each concentration (see concentrations table in DIP).
                string frictionAngle = s.ReadLine();            // Line contains n values, one value for each concentration (see concentrations table in DIP).
                string factor = s.ReadLine();                   // Line contains n values, one value for each concentration (see concentrations table in DIP).
                string typeWS = s.ReadLine();
                string factorWS = s.ReadLine();
                string dispersionCoeffs = s.ReadLine();

				double oneFactorWS;
				int oneTypeWS;
                double longitudinalDispCoeff;
                double transversalDispCoeff;

                //Density:
                {
					var row = new string[numberOfSediments + 1];
                    row[0] = Universal.Idioma("Density (","Densidad (") + (Universal.RiverUnits == "Metric" ? "kg/m3" : "lb/ft3") + ")"; 
					string[] split = density.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
					
					for (int i = 0; i < numberOfSediments; ++i)
						row[i + 1] = split[i];
					suspendedSedimentData.Add(row);
				}

				//Initial Concentration:
				{
					var row = new string[numberOfSediments + 1];
					string[] split = initialConcentration.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
					row[0] = Universal.Idioma("Initial Concentr.", "Concentr. Inicial");
					for (int i = 0 ; i < numberOfSediments ; ++i)
						row[i + 1] = split[i];
					suspendedSedimentData.Add(row);
				}

				//Diameter30:
				//{
				//	var row = new string[numberOfSediments + 1];
				//	string[] split = diameter30.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
				//	row[0] = "Diameter D30";
				//	for (int i = 0; i < numberOfSediments; ++i)
				//		row[i + 1] = split[i];
				//	suspendedSedimentData.Add(row);
				//}

				//Diameter50:
				{
					var row = new string[numberOfSediments + 1];
					string[] split = diameter50.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
					row[0] = Universal.Idioma("Diameter ", "Diámetro ") + units;
					for (int i = 0; i < numberOfSediments; ++i)
						row[i + 1] = split[i];
					suspendedSedimentData.Add(row);
				}

				//Diameter90:
				//{
				//	var row = new string[numberOfSediments + 1];
				//	string[] split = diameter90.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
				//	row[0] = "Diameter D90";
				//	for (int i = 0; i < numberOfSediments; ++i)
				//		row[i + 1] = split[i];
				//	suspendedSedimentData.Add(row);
				//}

				//Porosity:
				{
					var row = new string[numberOfSediments + 1];
					string[] split = porosity.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
					row[0] = Universal.Idioma("Porosity", "Porosidad");
					for (int i = 0; i < numberOfSediments; ++i)
						row[i + 1] = split[i];
					suspendedSedimentData.Add(row);
				}

				//Shear Stress:
				{
					var row = new string[numberOfSediments + 1];
					string[] split = criticalShieldStress.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
					row[0] = Universal.Idioma("Shear Stress", "Esfuerzo Cortante");
					for (int i = 0; i < numberOfSediments; ++i)
						row[i + 1] = split[i];
					suspendedSedimentData.Add(row);
				}

				//Friction Angle:
				{
					var row = new string[numberOfSediments + 1];
					string[] split = frictionAngle.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
					row[0] = Universal.Idioma( "Friction Angle", "Ángulo de Fricción");
					for (int i = 0; i < numberOfSediments; ++i)
						row[i + 1] = split[i];
					suspendedSedimentData.Add(row);
				}

				//Factor:
				{
					var row = new string[numberOfSediments + 1];
					string[] split = factor.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
					row[0] = "Factor";
					for (int i = 0; i < numberOfSediments; ++i)
						row[i + 1] = split[i];
					suspendedSedimentData.Add(row);
				}

				//Type WS:
				{
					string[] split = typeWS.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
					oneTypeWS = Convert.ToInt32(split[0]);
				}

				//Factor WS:
				{
					string[] split = factorWS.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
					oneFactorWS = Convert.ToDouble(split[0]);
				}

                //Dispersion Coeffs:
                {
                    if (dispersionCoeffs != null)
                    {
                        string[] split = dispersionCoeffs.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        longitudinalDispCoeff = Convert.ToDouble(split[0]);
                        transversalDispCoeff = Convert.ToDouble(split[1]);
                    }
                    else
                    {
                        longitudinalDispCoeff = 0;
                        transversalDispCoeff = 0;
                    }
                }

                //Invert rows and columns of suspendeSedimentData.
                var suspendedSedimentDataInverted = new List<string[]>();


				for (int i = 0; i < numberOfSediments + 1; i++)
				{
					var rowInverted = new string[suspendedSedimentData.Count];
					for (int j = 0; j < suspendedSedimentData.Count; j++)
					{
						rowInverted[j] = suspendedSedimentData[j][i];
					}
					suspendedSedimentDataInverted.Add(rowInverted);
				}

				//typeFormula = typeFormula - 1;
				//oneTypeWS = oneTypeWS - 1;
				AddVariable("SUSPENDED_SEDIMENT_PLUS_ACTIVE", activeSuspendedSediment);
				AddVariable("SUSPENDED_SEDIMENT_NUMBER_OF_SEDIMENTS", numberOfSediments);
				AddVariable("SUSPENDED_SEDIMENT_TYPE_FORMULA", typeFormula);
				AddVariable("SUSPENDED_SEDIMENT_TYPE_WS", oneTypeWS);
				AddVariable("SUSPENDED_SEDIMENT_FACTOR_WS", oneFactorWS);
				AddVariable("SUSPENDEDSEDIMENT_VALUES", suspendedSedimentDataInverted);
                AddVariable("SUSPENDED_SEDIMENT_LONGITUDINAL_DISP_COEFF", longitudinalDispCoeff);
                AddVariable("SUSPENDED_SEDIMENT_TRANSVERSAL_DISP_COEFF", transversalDispCoeff);
                SEDSLoaded = true;
			}
			catch
			{
				MessageBox.Show(Universal.Idioma("ERROR 1610131325: error trying to read .SEDS file.", "ERROR 1610131325: error leyendo archivo .SEDS."), 
                    "RiverFlow2D", MessageBoxButtons.OK,
				                MessageBoxIcon.Error);
				SEDSLoaded = false;
			}
			finally
			{
				s.Close();
			}
			

		}

		public void LoadSEDBFile(string fileName)
		{
			fileName = fileName.Remove(fileName.Length - 4) + "SEDB";

			if (!File.Exists(fileName))
			{
				MessageBox.Show(Universal.Idioma("File " + fileName + " doesn't exist.", "File " + fileName + " doesn't exist."),
                    "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
				SEDBLoaded = false;
				return;
			}

			var fileinfo = new FileInfo(fileName);
			if (fileinfo.Length < 1)
			{
				MessageBox.Show(Universal.Idioma("File " + fileName + " is empty.", "File " + fileName + " is empty."),
                    "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
				SEDBLoaded = false;
				return;
			}

			string units;
			if (Universal.RiverUnits == "Metric") units = "(mts)";
			else units = "(ft)";

			StreamReader s= File.OpenText(fileName);
			try
			{
				string read = s.ReadLine();
				int activeBedLoadSediment = Convert.ToInt32(read);

				read = s.ReadLine();
				int numberOfSediments = Convert.ToInt32(read);

				read = s.ReadLine();
				int typeFormula = Convert.ToInt32(read);

				var bedLoadSedimentData = new List<string[]>();

				string density = s.ReadLine();                   // Line contains n values, one value for each concentration (see concentrations table in DIP).
                string diameter30 = s.ReadLine();                // Line contains n values, one value for each concentration (see concentrations table in DIP).
                string diameter50 = s.ReadLine();                // Line contains n values, one value for each concentration (see concentrations table in DIP).
                string diameter90 = s.ReadLine();                // Line contains n values, one value for each concentration (see concentrations table in DIP).
                string porosity = s.ReadLine();                  // Line contains n values, one value for each concentration (see concentrations table in DIP).
                string criticalShieldStress = s.ReadLine();      // Line contains n values, one value for each concentration (see concentrations table in DIP).
                string frictionAngle = s.ReadLine();             // Line contains n values, one value for each concentration (see concentrations table in DIP).
                string fraction = s.ReadLine();                  // Line contains n values, one value for each concentration (see concentrations table in DIP).
                string factor = s.ReadLine();                    // Line contains n values, one value for each concentration (see concentrations table in DIP).

                string coupledComputation = s.ReadLine();

                //Density:
                {
					var row = new string[numberOfSediments + 1];
					string[] split = density.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
					row[0] = "Density (" + (Universal.RiverUnits == "Metric" ? "kg/m3" : "lb/ft3") + ")"; 
					for (int i = 0; i < numberOfSediments; ++i)
						row[i + 1] = split[i];
					bedLoadSedimentData.Add(row);
				}

				//Diameter30:
				{
					var row = new string[numberOfSediments + 1];
					string[] split = diameter30.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
					row[0] = "Diameter D30 " + units;

					for (int i = 0; i < numberOfSediments; ++i)
						row[i + 1] = split[i];
					bedLoadSedimentData.Add(row);
				}

				//Diameter50:
				{
					var row = new string[numberOfSediments + 1];
					string[] split = diameter50.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
					row[0] = "Diameter D50 " + units;
					for (int i = 0; i < numberOfSediments; ++i)
						row[i + 1] = split[i];
					bedLoadSedimentData.Add(row);
				}

				//Diameter90:
				{
					var row = new string[numberOfSediments + 1];
					string[] split = diameter90.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
					row[0] = "Diameter D90 " + units;
					for (int i = 0; i < numberOfSediments; ++i)
						row[i + 1] = split[i];
					bedLoadSedimentData.Add(row);
				}

				//Porosity:
				{
					var row = new string[numberOfSediments + 1];
					string[] split = porosity.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
					row[0] = Universal.Idioma("Porosity","Porosidad");
					for (int i = 0; i < numberOfSediments; ++i)
						row[i + 1] = split[i];
					bedLoadSedimentData.Add(row);
				}

				//Shear Stress:
				{
					var row = new string[numberOfSediments + 1];
					string[] split = criticalShieldStress.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
					row[0] = Universal.Idioma("Shear Stress", "Esfuerzo Cortante");
					for (int i = 0; i < numberOfSediments; ++i)
						row[i + 1] = split[i];
					bedLoadSedimentData.Add(row);
				}

				//Friction Angle:
				{
					var row = new string[numberOfSediments + 1];
					string[] split = frictionAngle.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
					row[0] = Universal.Idioma("Friction Angle", "Ángulo de Fricción");
					for (int i = 0; i < numberOfSediments; ++i)
						row[i + 1] = split[i];
					bedLoadSedimentData.Add(row);
				}

				//Fraction:
				{
					var row = new string[numberOfSediments + 1];
					string[] split = fraction.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
					row[0] = "Fraction";
					for (int i = 0; i < numberOfSediments; ++i)
						row[i + 1] = split[i];
					bedLoadSedimentData.Add(row);
				}

				//Factor:
				{
					var row = new string[numberOfSediments + 1];
					string[] split = factor.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
					row[0] = "Factor";
					for (int i = 0; i < numberOfSediments; ++i)
						row[i + 1] = split[i];
					bedLoadSedimentData.Add(row);
				}

				//Invert rows and columns of suspendeSedimentData.
				var bedLoadSedimentDataInverted = new List<string[]>();

				for (int i = 0; i < numberOfSediments + 1; i++)
				{
					var rowInverted = new string[bedLoadSedimentData.Count];
					for (int j = 0; j < bedLoadSedimentData.Count; j++)
					{
						rowInverted[j] = bedLoadSedimentData[j][i];
					}
					bedLoadSedimentDataInverted.Add(rowInverted);
				}

                //Coupled computation:
                int coupledComp;
                if (coupledComputation != null)
                {
                    coupledComp = Convert.ToInt16(coupledComputation);
                }
                else
                {
                    coupledComp = 1;
                }


                AddVariable("BED_LOAD_SEDIMENT_PLUS_ACTIVE", activeBedLoadSediment);
				AddVariable("BED_LOAD_SEDIMENT_NUMBER_OF_SEDIMENTS", numberOfSediments);
				AddVariable("BED_LOAD_SEDIMENT_TYPE_FORMULA", typeFormula);
                AddVariable("BED_LOAD_COUPLED_COMPUTATION", coupledComp);
				AddVariable("BEDLOADSEDIMENT_VALUES", bedLoadSedimentDataInverted);
                
                SEDBLoaded = true;
			}
			catch
			{
				MessageBox.Show(Universal.Idioma("ERROR 0211131106: error trying to read .SESB file.", "ERROR 0211131106: error leyendo archivo .SESB."), 
                    "RiverFlow2D", MessageBoxButtons.OK,
				                MessageBoxIcon.Error);
				SEDBLoaded = false;
			}

			s.Close();
		}

		//Save data to file.
		public void Save(string fileName)
		{
			SaveSEDSFile(fileName);
			SaveSEDBFile(fileName);
		}

		private void SaveSEDSFile(string fileName)
		{
			TextWriter w = new StreamWriter(fileName);

			//Suspended sediment.
			try
			{

				var suspendedSediments = (List<string[]>) GetVariable("SUSPENDEDSEDIMENT_VALUES");
				var numberOfSediments = (int) GetVariable("SUSPENDED_SEDIMENT_NUMBER_OF_SEDIMENTS");
				var suspendedSedimentActive = (int) GetVariable("SUSPENDED_SEDIMENT_PLUS_ACTIVE");
				var suspendedSedimentTypeFormula = (int) GetVariable("SUSPENDED_SEDIMENT_TYPE_FORMULA");
				var suspendedSedimentTypeWS = (int) GetVariable("SUSPENDED_SEDIMENT_TYPE_WS");
				var suspendedSedimentFactorWS = GetVariable("SUSPENDED_SEDIMENT_FACTOR_WS");
                var suspendedSedimentDispLongitudinalCoeff = GetVariable("SUSPENDED_SEDIMENT_LONGITUDINAL_DISP_COEFF");
                var suspendedSedimentDispTransversalCoeff = GetVariable("SUSPENDED_SEDIMENT_TRANSVERSAL_DISP_COEFF");


                if (numberOfSediments > 0)
				{
					w.WriteLine(suspendedSedimentActive);
					w.WriteLine(numberOfSediments);
					w.WriteLine((suspendedSedimentTypeFormula ).ToString());

					string line;

					//Density:
					line = "";
					for (int i = 1; i <= numberOfSediments; i++)
						line = line + suspendedSediments[i][0].Trim() + "  ";
					w.WriteLine(line);

					string fraction;
					if (Universal.RiverUnits == "Metric") fraction = "0.00000001";
					else fraction = "0.0000003937";

					//Initial concentration:
					line = "";
					for (int i = 1 ; i <= numberOfSediments ; i++)
						line = line + suspendedSediments[i][1].Trim() + "  ";
					w.WriteLine(line);

					//Diameter 30:
					line = "";
					for (int i = 1; i <= numberOfSediments; i++)
						line = line + fraction + "  ";
					w.WriteLine(line);

					//Diameter 50
					line = "";
					for (int i = 1; i <= numberOfSediments; i++)
						line = line + suspendedSediments[i][2].Trim() + "  ";
					w.WriteLine(line);

					//Diameter 90:
					line = "";
					for (int i = 1; i <= numberOfSediments; i++)
						line = line + fraction + "  ";
					w.WriteLine(line);

					//Porosity:
					line = "";
					for (int i = 1; i <= numberOfSediments; i++)
						line = line + suspendedSediments[i][3].Trim() + "  ";
					w.WriteLine(line);

					//Critical shear stress:
					line = "";
					for (int i = 1; i <= numberOfSediments; i++)
						line = line + suspendedSediments[i][4].Trim() + "  ";
					w.WriteLine(line);

					//Friction angle:
					line = "";
					for (int i = 1; i <= numberOfSediments; i++)
						line = line + suspendedSediments[i][5].Trim() + "  ";
					w.WriteLine(line);

					//Factor:
					line = "";
					for (int i = 1; i <= numberOfSediments; i++)
						line = line + suspendedSediments[i][6].Trim() + "  ";
					w.WriteLine(line);

					//Type WS:
					w.WriteLine(suspendedSedimentTypeWS.ToString());

					//Factor WS:
					w.WriteLine(suspendedSedimentFactorWS.ToString());

                    //Dispersion coeffs:
                    w.WriteLine(suspendedSedimentDispLongitudinalCoeff.ToString() + "  " + suspendedSedimentDispTransversalCoeff.ToString());

                }
				else
				{	
					MessageBox.Show(Universal.Idioma("The suspended sediment table is empty. It will be saved with one sediment with default values.", 
                        "La tabla de sedimentos suspendisos está vacía. Será almacenada con un sedimento con valores por defecto."), 
                        "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Information);
					//Suspended sediment is empty. Use defaults.
					w.WriteLine("0");
					w.WriteLine("1");
					w.WriteLine("1");
					if (Universal.RiverUnits == "English")
					{
						w.WriteLine("165");
						w.WriteLine("0.01");
						w.WriteLine("0.0000003937");
						w.WriteLine("0.0000003937");
						w.WriteLine("0.0000003937");
						w.WriteLine("0.4");
						w.WriteLine("0.047");
						w.WriteLine("35");
						w.WriteLine("1");
						w.WriteLine("1");
						w.WriteLine("1");
                        w.WriteLine("0  0");
                    }
					else
					{
						w.WriteLine("2650");
						w.WriteLine("0.01");
						w.WriteLine("0.00000001");
						w.WriteLine("0.00000001");
						w.WriteLine("0.00000001");
						w.WriteLine("0.4");
						w.WriteLine("0.047");
						w.WriteLine("35");
						w.WriteLine("1");
						w.WriteLine("1");
						w.WriteLine("1");
                        w.WriteLine("0  0");

                    }		
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(Universal.Idioma("ERROR 1110131853: error trying to save .SEDS file. ", "ERROR 1110131853: error almacenando archivo .SEDS. ") + 
                    ex.Message, "RiverFlow2D",
				                MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			w.Close();
		}

		private void SaveSEDBFile(string fileName)
		{

			fileName = fileName.Remove(fileName.Length - 4) + "SEDB";
			var w = new StreamWriter(fileName);

			try
			{

				var bedLoadSediments = (List<string[]>) GetVariable("BEDLOADSEDIMENT_VALUES");
				var numberOfSediments = (int) GetVariable("BED_LOAD_SEDIMENT_NUMBER_OF_SEDIMENTS");
				var bedLoadSedimentActive = (int) GetVariable("BED_LOAD_SEDIMENT_PLUS_ACTIVE");
				var bedLoadSedimentTypeFormula = (int) GetVariable("BED_LOAD_SEDIMENT_TYPE_FORMULA");
                var bedLoadSedimentCoupledComp = (int)GetVariable("BED_LOAD_COUPLED_COMPUTATION");

                if (numberOfSediments > 0)
				{

					w.WriteLine(bedLoadSedimentActive);
					w.WriteLine(numberOfSediments);
					w.WriteLine((bedLoadSedimentTypeFormula).ToString());

					string line;

					//Density:
					line = "";
					for (int i = 1 ; i <= numberOfSediments ; i++)
						line = line + bedLoadSediments[i][0].Trim() + "  ";
					w.WriteLine(line);

					//Diameter 30:
					line = "";
					for (int i = 1 ; i <= numberOfSediments ; i++)
						line = line + bedLoadSediments[i][1].Trim() + "  ";
					w.WriteLine(line);

					//Diameter 50
					line = "";
					for (int i = 1 ; i <= numberOfSediments ; i++)
						line = line + bedLoadSediments[i][2].Trim() + "  ";
					w.WriteLine(line);

					//Diameter 90:
					line = "";
					for (int i = 1 ; i <= numberOfSediments ; i++)
						line = line + bedLoadSediments[i][3].Trim() + "  ";
					w.WriteLine(line);

					//Porosity:
					line = "";
					for (int i = 1 ; i <= numberOfSediments ; i++)
						line = line + bedLoadSediments[i][4].Trim() + "  ";
					w.WriteLine(line);

					//Critical shear stress:
					line = "";
					for (int i = 1 ; i <= numberOfSediments ; i++)
						line = line + bedLoadSediments[i][5].Trim() + "  ";
					w.WriteLine(line);

					//Friction angle:
					line = "";
					for (int i = 1 ; i <= numberOfSediments ; i++)
						line = line + bedLoadSediments[i][6].Trim() + "  ";
					w.WriteLine(line);

					//Fraction:
					line = "";
					for (int i = 1 ; i <= numberOfSediments ; i++)
						line = line + bedLoadSediments[i][7].Trim() + "  ";
					w.WriteLine(line);
					
					//Factor:
					line = "";
					for (int i = 1 ; i <= numberOfSediments ; i++)
						line = line + bedLoadSediments[i][8].Trim() + "  ";
					w.WriteLine(line);

                    //Coupled Computation:
                    w.WriteLine(bedLoadSedimentCoupledComp);
                }
				else
				{
					MessageBox.Show(Universal.Idioma("The bed load table is empty. It will be saved with one sediment with default values.", 
                        "La tabla de transporte de carga de fondo está vacía. Será almacenada con un sedimento con valores por defecto."), 
                        "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Information);
					//Bed Load sediment is empty. Use defaults.
					w.WriteLine("0");
					w.WriteLine("1");
					w.WriteLine("1");
					if (Universal.RiverUnits == "English")
					{
						w.WriteLine("165");
						w.WriteLine("0.0036576");
						w.WriteLine("0.0036576");
						w.WriteLine("0.0036576");
						w.WriteLine("0.4");
						w.WriteLine("0.047");
						w.WriteLine("35");
						w.WriteLine("1");
						w.WriteLine("1");
                        w.WriteLine("1");
                    }
					else 
					{
						w.WriteLine("2650");
						w.WriteLine("0.001");
						w.WriteLine("0.001");
						w.WriteLine("0.001");
						w.WriteLine("0.4");
						w.WriteLine("0.047");
						w.WriteLine("35");
						w.WriteLine("1");
						w.WriteLine("1");
                        w.WriteLine("1");
                    }		
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(Universal.Idioma("ERROR 0211131113: error trying to save .SEDB file. ", "ERROR 0211131113: error almacenando archivo .SEDB. ") + 
                    ex.Message, "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			w.Close();

		}


		public static void AddColumnsToSuspendedSedimentTable(string fileName, DataGridView dataSuspendedSediment)
		{

			try
			{
				//Add required number of columns to dataSuspendedsediment dataGridView.
				StreamReader s = File.OpenText(fileName);
				string read = s.ReadLine();
				read = s.ReadLine(); //Number of sediments.
				for (int i = 1; i <= Convert.ToInt32(read); i++)
				{
					DataGridViewColumn col = new DataGridViewTextBoxColumn();
					col.Name = "Sediment" + (i).ToString();
					col.HeaderText = "Frac. " + (i).ToString();
					col.Width = 77;
					col.SortMode = DataGridViewColumnSortMode.NotSortable;
					dataSuspendedSediment.Columns.Add(col);
				}
				s.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(Universal.Idioma("ERROR 1811131612: error adding columns to suspended sediment table. ", "ERROR 1811131612: error agregando columnas a la tabla de sedimentos suspendidos. ") + 
                    ex.Message, "RiverFlow2D",
				                MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public static void AddColumnsToBedLoadSedimentTable(string fileName, DataGridView dataBedLoadSediment)
		{
			try
			{
				//Add required number of columns to dataBedLoadSediment dataGridView.
				StreamReader s;
				s = File.OpenText(fileName.Remove(fileName.Length - 4) + "SEDB");
				string read = s.ReadLine();
				read = s.ReadLine(); //Number of sediments.
				for (int i = 1 ; i <= Convert.ToInt32(read) ; i++)
				{
					DataGridViewColumn col = new DataGridViewTextBoxColumn();
					col.Name = "Sediment" + (i).ToString();
					col.HeaderText = "Frac. " + (i).ToString();
					col.Width = 63;
					col.SortMode = DataGridViewColumnSortMode.NotSortable;
					dataBedLoadSediment.Columns.Add(col);
				}
				s.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(Universal.Idioma("ERROR 1811131622: error adding columns to bed load sediment table. ", "ERROR 1811131622: error agregando columnas a la tabla de sedimentos. ") + 
                    ex.Message, "RiverFlow2D",
												MessageBoxButtons.OK, MessageBoxIcon.Error);
			}


		}


		public static void RemoveColumnsFromSuspendedSedimentTable(DataGridView dataSuspendedSediment, int fromColumn)
		{

			try
			{
				while (dataSuspendedSediment.Columns.Count > fromColumn)
				{
					string columnName;
					if (fromColumn == 0)
						columnName = dataSuspendedSediment.Columns[0].Name;
					else
						columnName = dataSuspendedSediment.Columns[1].Name;

					dataSuspendedSediment.Columns.Remove(columnName);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(Universal.Idioma("ERROR 1811131608: error while creating suspended sediment table. ", "ERROR 1811131608: error creando tabla de sedimentos suspendidos. ") + 
                    ex.Message, "RiverFlow2D",
								        MessageBoxButtons.OK, MessageBoxIcon.Error);			
			}
		
		}

		public static void RemoveColumnsFromBedLoadSedimentTable(DataGridView dataBedLoadSediment, int fromColumn)
		{
			try
			{
				while (dataBedLoadSediment.Columns.Count > fromColumn)
				{
					string columnName;
					if (fromColumn == 0)
						columnName = dataBedLoadSediment.Columns[0].Name;
					else
						columnName = dataBedLoadSediment.Columns[1].Name;

					dataBedLoadSediment.Columns.Remove(columnName);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(Universal.Idioma("ERROR 1811131609: error while creating bed load sediment table. ", "ERROR 1811131609: error creando tabla de sedimentos. ") + 
                    ex.Message, "RiverFlow2D",
								        MessageBoxButtons.OK, MessageBoxIcon.Error);			
			}
		}

		public static void DeploySuspendedSedimentHeaders(DataGridView dataSuspendedSediment)
		{
			try
			{
				string units;
				if (Universal.RiverUnits == "Metric")
					units = "(mts)";
				else
					units = "(ft)";

				if (dataSuspendedSediment.Rows.Count > 1)
				//if (origen == "For new project")
				{//skip adding rows.
					//MessageBox.Show("New Project");
				} 
				else
				{
					//Complete dataSuspendedSediment DatGridView:
					var row1 = (DataGridViewRow) dataSuspendedSediment.Rows[0].Clone();
					row1.Cells[0].Value = Universal.Idioma("Density (", "Densidad (") + (Universal.RiverUnits == "Metric" ? "kg/m3" : "lb/ft3") + ")";
					dataSuspendedSediment.Rows.Add(row1);

					var row2 = (DataGridViewRow) dataSuspendedSediment.Rows[0].Clone();
					row2.Cells[0].Value = Universal.Idioma("Initial Concentr.", "Conct. Inicial");
					dataSuspendedSediment.Rows.Add(row2);

					var row3 = (DataGridViewRow) dataSuspendedSediment.Rows[0].Clone();
					row3.Cells[0].Value = Universal.Idioma("Diameter D50 ","Diámetro D50") + units;
					dataSuspendedSediment.Rows.Add(row3);

					var row4 = (DataGridViewRow) dataSuspendedSediment.Rows[0].Clone();
					row4.Cells[0].Value = Universal.Idioma("Porosity", "Porosidad");
					dataSuspendedSediment.Rows.Add(row4);

					var row5 = (DataGridViewRow) dataSuspendedSediment.Rows[0].Clone();
					row5.Cells[0].Value = Universal.Idioma("Shear Stress", "Esfuerzo Cortante");
					dataSuspendedSediment.Rows.Add(row5);

					var row6 = (DataGridViewRow) dataSuspendedSediment.Rows[0].Clone();
					row6.Cells[0].Value = Universal.Idioma("Friction Angle", "Ángulo de Fricción");
					dataSuspendedSediment.Rows.Add(row6);

					var row7 = (DataGridViewRow) dataSuspendedSediment.Rows[0].Clone();
					row7.Cells[0].Value = "Factor";
					dataSuspendedSediment.Rows.Add(row7);
				}

				dataSuspendedSediment.AllowUserToAddRows = false;
				dataSuspendedSediment.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
				dataSuspendedSediment.ClearSelection();
				dataSuspendedSediment.Rows[1].Cells[0].Selected = true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(Universal.Idioma("ERROR 1411131423: deploying suspended sediment table . ", "ERROR 1411131423: mostrando tabla de sedimentos suapendidos . ") + 
                    ex.Message, "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);				
			}

		}

		public static void DeployBedLoadSedimentHeaders(DataGridView dataBedLoadSediment)
		{
			try
			{
				string units;
				if (Universal.RiverUnits == "Metric")
					units = "mts";
				else
					units = "ft";

				if (dataBedLoadSediment.Rows.Count > 1)
				//if (origen == "For new project")
				{
					//Skip Headers.
				}
				else
				{
					//Complete dataBedLoaddSediment DataGridView:
					var row1 = (DataGridViewRow) dataBedLoadSediment.Rows[0].Clone();
					row1.Cells[0].Value = Universal.Idioma("Density (", "Densidad (") + (Universal.RiverUnits == "Metric" ? "kg/m3" : "lb/ft3") + ")";
					dataBedLoadSediment.Rows.Add(row1);

					var row2 = (DataGridViewRow) dataBedLoadSediment.Rows[0].Clone();
					row2.Cells[0].Value = Universal.Idioma("Diameter D30 ", "Diámetro D30 ") + "(" + units + ")";
					dataBedLoadSediment.Rows.Add(row2);

					var row3 = (DataGridViewRow) dataBedLoadSediment.Rows[0].Clone();
					row3.Cells[0].Value = Universal.Idioma("Diameter D50 ", "Diámetro D50 ") + "(" + units + ")";
					dataBedLoadSediment.Rows.Add(row3);

					var row4 = (DataGridViewRow) dataBedLoadSediment.Rows[0].Clone();
					row4.Cells[0].Value = Universal.Idioma("Diameter D90 ", "Diámetro D90") + "(" + units + ")";
					dataBedLoadSediment.Rows.Add(row4);

					var row5 = (DataGridViewRow) dataBedLoadSediment.Rows[0].Clone();
					row5.Cells[0].Value = Universal.Idioma("Porosity","Porosidad");
					dataBedLoadSediment.Rows.Add(row5);

					var row6 = (DataGridViewRow) dataBedLoadSediment.Rows[0].Clone();
					row6.Cells[0].Value = Universal.Idioma("Shear Stress", "Esfuerzo Cortante");
					dataBedLoadSediment.Rows.Add(row6);

					var row7 = (DataGridViewRow) dataBedLoadSediment.Rows[0].Clone();
					row7.Cells[0].Value = Universal.Idioma("Friction Angle", "Ángulo de Fricción");
                    dataBedLoadSediment.Rows.Add(row7);

					var row8 = (DataGridViewRow) dataBedLoadSediment.Rows[0].Clone();
					row8.Cells[0].Value = Universal.Idioma("Fraction", "Fracción");
					dataBedLoadSediment.Rows.Add(row8);

					var row9 = (DataGridViewRow) dataBedLoadSediment.Rows[0].Clone();
					row9.Cells[0].Value = "Factor";
					dataBedLoadSediment.Rows.Add(row9);
				}

				dataBedLoadSediment.AllowUserToAddRows = false;
				dataBedLoadSediment.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
				dataBedLoadSediment.ClearSelection();
				dataBedLoadSediment.Rows[0].Cells[0].Selected = true;

			}
			catch (Exception ex)
			{
				MessageBox.Show(Universal.Idioma("ERROR 1655131655: deploying bed load sediment table . ", "ERROR 1655131655: mostrando tabla de sedimentos . ") + 
                    ex.Message, 
                    "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);				
			}
		}	
	}
}



