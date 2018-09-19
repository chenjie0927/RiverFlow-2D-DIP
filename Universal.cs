using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System;

namespace Simulation_Options
{
    class Universal : InputFile
    {

        public static string DIPLanguage; 

        public static bool EnableTextChangedEvent;
		// New comment to text new file .gitignore
		// And now a new one

        public static List<string[]> Solutes = new List<string[]>();
        public static string[] RecentProjects = new string[5];
        public static string OpenProjectExit = "";
        public static string ProjectPathAndName = "";

        //CULVERT data.....

        public struct CulvertValuesGroup
        {
            public int Type;
            public string FileName;
            public long NPoints;
            public SecondaryTable[] Table;
            public string Nb;
            public string nc;
            public string Ke;
            public string Kp;
            public string M;
            public string cp;
            public string Y;
            public string m; //inlet form coefficient
            public string Hb;
            public string Base;
            public string Dc;

            public string Z1;
            public string Z2;
            public int ComboManningSelectionIndex;
            public int ComboEntranceLossSelectionIndex;
            public int ComboInletCoeffsSelectionIndex;
            public int UseElementElevations;
        }


        public static readonly Dictionary<string, CulvertValuesGroup> CulvertValuesGroups =
                           new Dictionary<string, CulvertValuesGroup>();

        //LRAIN data.....


        public struct SecondaryTable
        {
            public string Col0;
            public string Col1;
            public string Col2;
            public string Col3;
            public string Col4;
            public string Col5;
            public string Col6;
            public string Col7;
            public string Col8;
            public string Col9;
            public string Col10;
            public string Col11;
            public string Col12;
            //public string Col13;
            //public string Col14;
        }

        public struct SecondaryGroup
        {
            public string Component;
            public string FileName;
            public long NPoints;
            public SecondaryTable[] Table;
        }


        public static readonly Dictionary<string, SecondaryGroup> SecondaryGroups =
                           new Dictionary<string, SecondaryGroup>();

        //LINF data..... (Infiltration)
        public struct InfiltrationParametersGroup
        {
            public string ParametersFileName;
            public string ModelType;
            public string DecayRate;
            public string FinalRate;
            public string InitialRate;
            public string HydraulicConductivity;
            public string PsiWetting;
            public string DeltaTheta;
            public string CNCurveNumber;
            public string InitialAbstraction;
        }

        //InfiltrationParametersFile contains in memory just one copy of the contents of infiltration parameters files.
        //The same file could be referenced by an infiltration entry in the .LINF file 
        public static readonly Dictionary<string, InfiltrationParametersGroup> InfiltrationParametersFiles =
                               new Dictionary<string, InfiltrationParametersGroup>();

        public static Color BackColorError = Color.FromArgb(255, 255, 180, 180);

        public enum PdfPageNumber
        {
            DAT = 9,
            FED = 15,
            IFL = 17,
            PLT = 21,
            PRF = 24
        }

        public static string RiverUnits;

        public struct Rectangle
        {
            public double Left;
            public double Right;
            public double Bottom;
            public double Top;
        }

        public static Rectangle Domain;

        public static string ConfigConfig;

        public static bool InDomain(double x, double y)
        {
            return x >= Domain.Left & x <= Domain.Right & y >= Domain.Bottom & y <= Domain.Top;
        }

        public static void CreateSecondaryDefaultFile(string component, string fileName, int nValues)
        {
            var aOneLineTable = new SecondaryTable[1];
            string aSingleLine = "";
            switch (nValues)
            {
                case 1:
                    aOneLineTable[0].Col0 = "0";
                    aSingleLine = "0";
                    break;

                case 2:
                    aOneLineTable[0].Col0 = "0";
                    aOneLineTable[0].Col1 = "0";
                    aSingleLine = "0 0";
                    break;

                case 3:
                    aOneLineTable[0].Col0 = "0";
                    aOneLineTable[0].Col1 = "0";
                    aOneLineTable[0].Col2 = "0";
                    aSingleLine = "0 0 0";
                    break;

                case 4:
                    aOneLineTable[0].Col0 = "0";
                    aOneLineTable[0].Col1 = "0";
                    aOneLineTable[0].Col2 = "0";
                    aOneLineTable[0].Col3 = "0";
                    aSingleLine = "0 0 0 0";
                    break;

                case 5:
                    aOneLineTable[0].Col0 = "0";
                    aOneLineTable[0].Col1 = "0";
                    aOneLineTable[0].Col2 = "0";
                    aOneLineTable[0].Col3 = "0";
                    aOneLineTable[0].Col4 = "0";
                    aSingleLine = "0 0 0 0 0";
                    break;

                case 6:
                    aOneLineTable[0].Col0 = "0";
                    aOneLineTable[0].Col1 = "0";
                    aOneLineTable[0].Col2 = "0";
                    aOneLineTable[0].Col3 = "0";
                    aOneLineTable[0].Col4 = "0";
                    aOneLineTable[0].Col5 = "0";
                    aSingleLine = "0 0 0 0 0 0";
                    break;

                case 7:
                    aOneLineTable[0].Col0 = "0";
                    aOneLineTable[0].Col1 = "0";
                    aOneLineTable[0].Col2 = "0";
                    aOneLineTable[0].Col3 = "0";
                    aOneLineTable[0].Col4 = "0";
                    aOneLineTable[0].Col5 = "0";
                    aOneLineTable[0].Col6 = "0";
                    aSingleLine = "0 0 0 0 0 0 0";
                    break;

                case 8:
                    aOneLineTable[0].Col0 = "0";
                    aOneLineTable[0].Col1 = "0";
                    aOneLineTable[0].Col2 = "0";
                    aOneLineTable[0].Col3 = "0";
                    aOneLineTable[0].Col4 = "0";
                    aOneLineTable[0].Col5 = "0";
                    aOneLineTable[0].Col6 = "0";
                    aOneLineTable[0].Col7 = "0";
                    aSingleLine = "0 0 0 0 0 0 0 0";
                    break;
            }

            var data = new SecondaryGroup();
            data.Component = component;
            data.FileName = fileName;
            data.NPoints = 1;
            data.Table = aOneLineTable;

            SecondaryGroups.Add(fileName, data);

            // Write data.
            TextWriter file = new StreamWriter(fileName);
            file.WriteLine("1");
            file.WriteLine(aSingleLine);
            file.Close();
        }

        public static void LoadSecondaryTable(string component, string secondaryPathAndFileName, ref List<string> warnings)
        {
            //Create new item in SecondaryGroups structure (if it is not 
            //already there) with data from secondaryPathAndFileName.

            if (File.Exists(secondaryPathAndFileName))
            {
                if (SecondaryGroups.ContainsKey(secondaryPathAndFileName))
                {
                    SecondaryGroups.Remove(secondaryPathAndFileName);
                    //NOTE: the above comment is not valid. Keep it until debugging!!!!!:
                    //Do not load the data from this file since it is already in memory.
                    //In memory it could have been modified but we want to keep it as it 
                    //is, just as Word and Excel does when a file is loaded again.
                }

                StreamReader secondaryFile;
                secondaryFile = File.OpenText(secondaryPathAndFileName);
                try
                {
                    var thisGroup = new SecondaryGroup();
                    thisGroup.Component = component;
                    thisGroup.FileName = secondaryPathAndFileName;
                    thisGroup.NPoints = Convert.ToInt64(secondaryFile.ReadLine());
                    var thisTable = new SecondaryTable[thisGroup.NPoints];

                    for (int j = 0; j <= thisGroup.NPoints - 1; ++j)
                    {
                        string next = secondaryFile.ReadLine();
                        string[] splt = next.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        int nColumns = splt.Length;
                        switch (nColumns)
                        {
                            case 1:
                                thisTable[j].Col0 = splt[0];
                                break;

                            case 2:
                                thisTable[j].Col0 = splt[0];
                                thisTable[j].Col1 = splt[1];
                                break;

                            case 3:
                                thisTable[j].Col0 = splt[0];
                                thisTable[j].Col1 = splt[1];
                                thisTable[j].Col2 = splt[2];
                                break;

                            case 4:
                                thisTable[j].Col0 = splt[0];
                                thisTable[j].Col1 = splt[1];
                                thisTable[j].Col2 = splt[2];
                                thisTable[j].Col3 = splt[3];
                                break;

                            case 5:
                                thisTable[j].Col0 = splt[0];
                                thisTable[j].Col1 = splt[1];
                                thisTable[j].Col2 = splt[2];
                                thisTable[j].Col3 = splt[3];
                                thisTable[j].Col4 = splt[4];
                                break;

                            case 6:
                                thisTable[j].Col0 = splt[0];
                                thisTable[j].Col1 = splt[1];
                                thisTable[j].Col2 = splt[2];
                                thisTable[j].Col3 = splt[3];
                                thisTable[j].Col4 = splt[4];
                                thisTable[j].Col5 = splt[5];
                                break;

                            case 7:
                                thisTable[j].Col0 = splt[0];
                                thisTable[j].Col1 = splt[1];
                                thisTable[j].Col2 = splt[2];
                                thisTable[j].Col3 = splt[3];
                                thisTable[j].Col4 = splt[4];
                                thisTable[j].Col5 = splt[5];
                                thisTable[j].Col6 = splt[6];
                                break;

                            case 8:
                                thisTable[j].Col0 = splt[0];
                                thisTable[j].Col1 = splt[1];
                                thisTable[j].Col2 = splt[2];
                                thisTable[j].Col3 = splt[3];
                                thisTable[j].Col4 = splt[4];
                                thisTable[j].Col5 = splt[5];
                                thisTable[j].Col6 = splt[6];
                                thisTable[j].Col7 = splt[7];
                                break;
                        }

                    }

                    secondaryFile.Close();

                    //Store bridge profile table:
                    thisGroup.Table = thisTable;

                    //Store this new group:
                    SecondaryGroups.Add(secondaryPathAndFileName, thisGroup);
                }
                catch (Exception ex)
                {
                    secondaryFile.Close();
                    MessageBox.Show(Idioma("ERROR 0605171628: error reading file ", "ERROR 0605171628: error leyendo archivo ") + "\n\n" + secondaryPathAndFileName + "\n\n" + ex.Message, 
                        "RiverFlow2D", MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);

                    //Create a Default empty file.
                    CreateSecondaryDefaultFile(component, secondaryPathAndFileName, 5);
                    if (warnings.IndexOf(secondaryPathAndFileName) == -1)
                    {
                        warnings.Add(secondaryPathAndFileName);
                    }
                }
            }
            else
            {
                //secondaryPathAndFileName doesn't exist. Create a Default empty file.
                CreateSecondaryDefaultFile(component, secondaryPathAndFileName, 5);
                if (warnings.IndexOf(secondaryPathAndFileName) == -1)
                {
                    warnings.Add(secondaryPathAndFileName);
                }
            }
        }

        public static void SaveSecondaryTables(string component, int nColumns)
        {
            foreach (SecondaryGroup aGroup in SecondaryGroups.Values)
            {
                if (aGroup.Component == component)
                {
                    nColumns = VisibleColumnsInSecondaryGroup(aGroup.FileName);
                    if (nColumns > 0)
                    {
                        TextWriter secondaryFile = new StreamWriter(aGroup.FileName);
                        try
                        {
                            secondaryFile.WriteLine(aGroup.NPoints);
                            for (int i = 0; i < aGroup.NPoints; i++)
                            {
                                switch (nColumns)
                                {
                                    case 1:
                                        secondaryFile.WriteLine(aGroup.Table[i].Col0);
                                        break;

                                    case 2:
                                        secondaryFile.WriteLine(aGroup.Table[i].Col0 + "   " +
                                                                aGroup.Table[i].Col1);
                                        break;

                                    case 3:
                                        secondaryFile.WriteLine(aGroup.Table[i].Col0 + "   " +
                                                                aGroup.Table[i].Col1 + "   " +
                                                                aGroup.Table[i].Col2);
                                        break;

                                    case 4:
                                        secondaryFile.WriteLine(aGroup.Table[i].Col0 + "   " +
                                                                aGroup.Table[i].Col1 + "   " +
                                                                aGroup.Table[i].Col2 + "   " +
                                                                aGroup.Table[i].Col3);
                                        break;

                                    case 5:
                                        secondaryFile.WriteLine(aGroup.Table[i].Col0 + "   " +
                                                                aGroup.Table[i].Col1 + "   " +
                                                                aGroup.Table[i].Col2 + "   " +
                                                                aGroup.Table[i].Col3 + "   " +
                                                                aGroup.Table[i].Col4);
                                        break;

                                    case 6:
                                        secondaryFile.WriteLine(aGroup.Table[i].Col0 + "   " +
                                                                aGroup.Table[i].Col1 + "   " +
                                                                aGroup.Table[i].Col2 + "   " +
                                                                aGroup.Table[i].Col3 + "   " +
                                                                aGroup.Table[i].Col4 + "   " +
                                                                aGroup.Table[i].Col5);
                                        break;

                                    case 7:
                                        secondaryFile.WriteLine(aGroup.Table[i].Col0 + "   " +
                                                                aGroup.Table[i].Col1 + "   " +
                                                                aGroup.Table[i].Col2 + "   " +
                                                                aGroup.Table[i].Col3 + "   " +
                                                                aGroup.Table[i].Col4 + "   " +
                                                                aGroup.Table[i].Col5 + "   " +
                                                                aGroup.Table[i].Col6);
                                        break;

                                    case 8:
                                        secondaryFile.WriteLine(aGroup.Table[i].Col0 + "   " +
                                                                aGroup.Table[i].Col1 + "   " +
                                                                aGroup.Table[i].Col2 + "   " +
                                                                aGroup.Table[i].Col3 + "   " +
                                                                aGroup.Table[i].Col4 + "   " +
                                                                aGroup.Table[i].Col5 + "   " +
                                                                aGroup.Table[i].Col6 + "   " +
                                                                aGroup.Table[i].Col7);

                                        break;
                                }

                            }
                            secondaryFile.Close();
                        }

                        catch (Exception ex)
                        {
                            secondaryFile.Close();
                            MessageBox.Show(Universal.Idioma("ERROR 0609170411: error saving file ", "ERROR 0609170411: error almacenando archivo ") + 
                                "\n\n" + secondaryFile + "\n\n" + ex.Message, "RiverFlow2D", MessageBoxButtons.OK,
                                            MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        public static int VisibleColumnsInSecondaryGroup(string fileName)
        {
            //How many additional columns are there in the SOURCES secondary group:
            int nCurrentVisibleColumns = 0;

            try
            {
                SecondaryGroup aSeries = SecondaryGroups[fileName];
                if (aSeries.Table[0].Col0 != null)
                    nCurrentVisibleColumns++;
                if (aSeries.Table[0].Col1 != null)
                    nCurrentVisibleColumns++;
                if (aSeries.Table[0].Col2 != null)
                    nCurrentVisibleColumns++;
                if (aSeries.Table[0].Col3 != null)
                    nCurrentVisibleColumns++;
                if (aSeries.Table[0].Col4 != null)
                    nCurrentVisibleColumns++;
                if (aSeries.Table[0].Col5 != null)
                    nCurrentVisibleColumns++;
                if (aSeries.Table[0].Col6 != null)
                    nCurrentVisibleColumns++;
                if (aSeries.Table[0].Col7 != null)
                    nCurrentVisibleColumns++;
                if (aSeries.Table[0].Col8 != null)
                    nCurrentVisibleColumns++;
                if (aSeries.Table[0].Col9 != null)
                    nCurrentVisibleColumns++;
                if (aSeries.Table[0].Col10 != null)
                    nCurrentVisibleColumns++;
                if (aSeries.Table[0].Col11 != null)
                    nCurrentVisibleColumns++;
                if (aSeries.Table[0].Col12 != null)
                    nCurrentVisibleColumns++;
            }
            catch (Exception ex)
            {
                MessageBox.Show(Universal.Idioma("ERROR 0909170615: error while computing visible columns. ", "ERROR 0909170615: error calculando columnas visibles. ") + 
                    ex.Message, "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return nCurrentVisibleColumns;
        }

        public static string Idioma(string txtEnglish, string txtSpanish)
        {
            if (txtSpanish == "")
                return txtEnglish;
            else if (DIPLanguage == "English")
                return txtEnglish;
            else
                return txtSpanish;
        }

        public static string Idioma(string txtEnglish)
        {
                return txtEnglish;
        }

    }
}

