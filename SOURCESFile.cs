using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Simulation_Options
{
    class SOURCESFile : InputFile
    {
        //public string SOURCESDirectory;
        //public string SOURCESFilePathAndName;

        public SOURCESFile()
        {
	        var emptySOURCES = new List<string[]>();
	        AddVariable("SOURCES_VALUES", emptySOURCES);
	        AddVariable("NUMBER_OF_SOURCES", 0);
            AddVariable("SOURCES_SERIES_VALUES", emptySOURCES);
        }

        public class SOURCES  //Stores a copy of the .SOURCES file
        {
            public string SourceName;
            public string SourceType;
            public string SecondaryFileName;
            public string XY;
        };

        public List<SOURCES> SOURCESFileContents = new List<SOURCES>();

        //Load .SOURCES data from file
        public virtual void Load(string SOURCESFileName)
        {
            StreamReader SOURCESFile = File.OpenText(SOURCESFileName);
            var warnings = new List<string>();

            int index = SOURCESFileName.LastIndexOf("\\", StringComparison.Ordinal);
            string SOURCESFilePath = SOURCESFileName.Remove(index);

            //Number of sources groups in .SOURCES file:
            string nGroups = SOURCESFile.ReadLine();
            int numberOfGroups = Convert.ToInt32(nGroups);
            AddVariable("NUMBER_OF_SOURCES", numberOfGroups);

            //Reset data structs to store .LRAIN data in local memory:
            var mainTable = new List<string[]>();
            SOURCESFileContents.Clear();

            //
            //(Code space for individual variables)
            //

            for (int i = 0; i < numberOfGroups; ++i)
            { //Read all rest of data from .SOURCES file....
                try
                {   string secondaryTableFileName;
                    //
                    //
                    //
                    //
                    //
                    //  Empty lines left intentionally
                    //Source name:
                    string sourceName = SOURCESFile.ReadLine().Trim();
                    //Source type:
                    string sourceType = SOURCESFile.ReadLine().Trim();
                    if (!int.TryParse(sourceType, out int intType))
                    {
                        // Expecting an integer. Maybe it is version previous to 2018.
                        secondaryTableFileName = sourceType;
                        sourceType = "1";
                    }
                    else
                    {
                        //Secondary table file name:
                        secondaryTableFileName = SOURCESFile.ReadLine().Trim();
                    }

                    //X Y:
                    string xy = SOURCESFile.ReadLine().Trim();
                    string[] split = xy.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    string X = split[0];
                    string Y = split[1];

                    //Create this group from data just read:
                    var group = new SOURCES();
                    group.SourceName = sourceName;
                    group.SourceType = sourceType;
                    group.SecondaryFileName = secondaryTableFileName;
                    group.XY = xy;

                    //Store data of this zone:
                    SOURCESFileContents.Add(group);

                    //Store in mainTable a row to be shown in zones table in DIP tab: 
                    //(index, rain/evaporation hydrograph)
                    var row = new string[5];
                    row[0] = sourceName;
                    row[1] = sourceType;
                    row[2] = secondaryTableFileName;
                    row[3] = X;
                    row[4] = Y;

                    mainTable.Add(row);

                    //Potential new sources group (secundary dependant table). Sources data  (time, Elevations, Concentrations (various)) is 
                    // stored in a file whose name could be repeated in the .SOURCES file for another group. Only one copy of the file contents is 
                    //stored in memory in SecondaryGroups structure. If modified, only that copy is changed. It is permanently stored when 
                    //the user clicks "Save .SOURCES".

                    Universal.LoadSecondaryTable("SOURCES", SOURCESFilePath + "\\" + secondaryTableFileName, ref warnings);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(Universal.Idioma("ERROR 0208171811: error while proccessing  ", "ERROR 0208171811: error procesando  ") + 
                        SOURCESFileName + ". " +
                                        Environment.NewLine + ex.Message, "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            SOURCESFile.Close();

            //Save variables associated to control tags:
            AddVariable("SOURCES_VALUES", mainTable);
            //
            //
            //
            //  Empty lines left intentionally

            if (warnings.Count > 0)
            {
                string warningsList = Universal.Idioma("WARNING 0208171813: The following sources files do not exist: \n\n", "WARNING 0208171813: Los siguientes archivos no existen: \n\n") ;
                for (int i = 0; i < warnings.Count; i++)
                {
                    warningsList += "   ° " + warnings[i] + "\n";
                }
                warningsList += Universal.Idioma("\nDefault files were created.","\nAchivos por defecto fueron creados.");
                MessageBox.Show(warningsList, "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
    }

    //Save data to file
    public virtual void Save(string SOURCESfilename)
        {
            try
            {
                if (SOURCESFileContents.Count > 0)
                {

                    {//Write .SOURCES file:
                        TextWriter w = new StreamWriter(SOURCESfilename);
                        //number of sources:
                        w.WriteLine(SOURCESFileContents.Count);


                        for (int i = 0; i < SOURCESFileContents.Count; ++i)
                        {
                            //Sources name, file name, XY:
                            w.WriteLine(SOURCESFileContents[i].SourceName);
                            w.WriteLine(SOURCESFileContents[i].SourceType);
                            w.WriteLine(SOURCESFileContents[i].SecondaryFileName);
                            w.WriteLine(SOURCESFileContents[i].XY);
                        }
                        w.Close();
                    }

                    // Write secondary sources data files.
                    int nColumns = 3;
                    Universal.SaveSecondaryTables("SOURCES", nColumns);
                }
                else
                {
                    MessageBox.Show(Universal.Idioma("The sources table is empty. It was not saved.", "La tabla de fuentes y sumideros está vacía. No fue almacenada."), 
                        "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                MessageBox.Show(Universal.Idioma("ERROR 0308171717: error while saving .SOURCES file.", "ERROR 0308171717: error almacenando archivo .SOURCES."), 
                    "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //read file names
        public string[] ReadFileNames(string filename)
        {
            StreamReader s = File.OpenText(filename);
            string read = s.ReadLine();
            int numberOfSOURCES = Convert.ToInt32(read);

            var fileNames = new string[numberOfSOURCES];

            try
            {
	            new List<string[]>();
	            for (int i = 0; i < numberOfSOURCES; ++i)
                {
                    //source name
                    s.ReadLine();
                    //source type
                    s.ReadLine();
					//name of file
                    fileNames[i] = s.ReadLine();
                    //x y
                    string line = s.ReadLine();
                }
            }
            catch
            {
                MessageBox.Show(Universal.Idioma("ERROR 1412110624: error trying to read .SOURCES file.", "ERROR 1412110624: error leyendo archivo .SOURCES."), 
                    "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            s.Close();

            return fileNames;

        }
    }
}
