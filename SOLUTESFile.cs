using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace Simulation_Options
{
	class SOLUTESFile : InputFile
	{

		public SOLUTESFile()
		{
			var emptySolutes = new List<string[]>();
			AddVariable("POLLUTANTTRANSPORTPLUS_VALUES", emptySolutes);
			AddVariable("NUMBER_OF_SOLUTES", 0);
		}

		//Load data from file
		public virtual void Load(string filename)
		{
			//Load all data into the data manager
			StreamReader s = File.OpenText(filename);
           
			try
			{
				string read = s.ReadLine();
				int numberOfSolutes = Convert.ToInt32(read);
				AddVariable("NUMBER_OF_SOLUTES", numberOfSolutes);

				string nSelected = s.ReadLine();
				string solutesSelected = s.ReadLine();
				string line = s.ReadLine();
				string[] coeffs = line.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
				string longDispCoeff = coeffs[0];
				string transvDispCoeff = coeffs[1];
				var row = new string[numberOfSolutes,3];
               
				var pollutantTransportPlusData = new List<string[]>();

				for (int i = 0; i < numberOfSolutes; ++i)
				{
					string soluteName = s.ReadLine();
					if (solutesSelected.Contains((i + 1).ToString()))
						row[i, 0] = "True";
					else
						row[i, 0] = "False";
					row[i, 1] = soluteName;
				}

				//Read line with reaction coefficients.
				line = s.ReadLine();
				string[] reactionCoefficients = line.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < numberOfSolutes; ++i)
				{
					row[i, 2] = reactionCoefficients[i];
					var thisRow = new string[3];
					thisRow[0] = row[i, 0];
					thisRow[1] = row[i, 1];
					thisRow[2] = row[i, 2];
					pollutantTransportPlusData.Add(thisRow);
				}

                

				AddVariable("POLL_TRANSP_PLUS_LONGITUDINAL_DISP_COEFF", longDispCoeff);
				AddVariable("POLL_TRANSP_PLUS_TRANSVERSAL_DISP_COEFF", transvDispCoeff);
				AddVariable("POLLUTANTTRANSPORTPLUS_VALUES", pollutantTransportPlusData);

			}
			catch
			{
				MessageBox.Show(Universal.Idioma("ERROR 0703141750: error trying to read .SOLUTES file.", "ERROR 0703141750: error leyendo archivo .SOLUTES."), 
                    "RiverFlow2D", MessageBoxButtons.OK,
				                MessageBoxIcon.Error);
			}
			finally
			{
				s.Close();
			}

		}

		//Save data to file
		public virtual void Save(string filename)
		{
			try
			{
				var solutes = (List<string[]>) GetVariable("POLLUTANTTRANSPORTPLUS_VALUES");
				int numberOfSolutes = solutes.Count;
				var longDispCoeff = (String) GetVariable("POLL_TRANSP_PLUS_LONGITUDINAL_DISP_COEFF");
				var transDispCoeff = (string) GetVariable("POLL_TRANSP_PLUS_TRANSVERSAL_DISP_COEFF");

                if (string.IsNullOrEmpty(longDispCoeff)) longDispCoeff = "1";
                if (string.IsNullOrEmpty(transDispCoeff)) transDispCoeff = "1";
                   
				if (numberOfSolutes > 0)
				{
					string solutesSelected = "";
					string reactionCoeffs = "";
					int  nSelected = 0;
					bool allNamesGiven = true;
					bool allReactionCoeffGiven = true;

					for (int j = 0 ; j < solutes.Count ; ++j)
					{
						string selected = solutes[j][0];
						if (selected == "True")
						{
							selected = (j+1).ToString();
							nSelected++;
							solutesSelected = solutesSelected + " " + selected;
						}
						
						if (((string) solutes[j][1]) == "") allNamesGiven = false;
						if (((string) solutes[j][2]) == "") allReactionCoeffGiven = false;

						//Make reaction coefficients string to write in one line.
						reactionCoeffs = reactionCoeffs + " " + solutes[j][2];
					}

					if (nSelected > 0)
					{
						if (allNamesGiven)
						{
							if (allReactionCoeffGiven)
							{
								TextWriter w = new StreamWriter(filename);
								//number of solutes
								w.WriteLine(numberOfSolutes);
								w.WriteLine(nSelected);
								w.WriteLine(solutesSelected);
								w.WriteLine(longDispCoeff + " " + transDispCoeff);

								for (int j = 0; j < solutes.Count; ++j)
								{
									w.WriteLine((string) solutes[j][1]);
								}
								w.WriteLine(reactionCoeffs);
								w.Close();
							}
							else
							{
								MessageBox.Show(Universal.Idioma("All reaction coefficients in the pollutant transport table must be given. It was not saved.", 
                                    "Todos los coeficientes de reacción en la tabla de calidad del agua deben darse. No fue almacenada."), 
                                    "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Information);
							}
						}
						else
						{
							MessageBox.Show(Universal.Idioma("All solute names in the pollutant transport table must be given. It was not saved.",
                                "Todos los nombres en la tabla de calidad del agua deben darse. No fue almacenada."), 
                                "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Information);
						}
					}
					else
					{
						MessageBox.Show(Universal.Idioma("At least one solute must be selected in the pollutant transport table. It was not saved.", 
                            "Al menos una fracción debe ser seleccionada en la tabla de calidad del agua. No fue almacenada"), 
                            "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Information);						
					}
				}
				else
				{
					MessageBox.Show(Universal.Idioma("The pollutant transport table is empty. It was not saved.", "La tabla de calidad del agua está vacía. No fue almacenada."),
                        "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
			catch (Exception ex) 
			{
				MessageBox.Show(Universal.Idioma("ERROR 1003141036: error trying to save .SOLUTES file. ", "ERROR 1003141036: error almacenando archivo .SOLUTES. ") + 
                    ex.Message, "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}

