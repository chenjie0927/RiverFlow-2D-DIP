using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Simulation_Options
{
    
    enum DataTypes
    {
        Integer,
        Double,
        String,
        IntegerList,
        DoubleList,
        List,
        Custom
    }

    class MixedList
    {
        private readonly List<DataTypes> _types;
        private List<Object> _data;

        public MixedList()
        {
            _types = new List<DataTypes>();
	        _data = new List<Object>();
        }

        public void AddNewFieldType(DataTypes type)
        {
            _types.Add(type);
        }

        public DataTypes GetFieldType(int fieldNumber)
        {
            return _types[fieldNumber];
        }

        public int NumberOfFields()
        {
            return _types.Count;
        }
    }

    class ValFile
    {
        // to write RiverFLO2Ddata.VAL
        public static string[] RiverFLO2DdataVal = new string[200];
        public static int ValIndex = 0;
        public static bool SaveRiverFLO2DdataVal = false;

    }

    class DataNode
    {
        //Name of Variable
        public string Id, Dependent;
        public DataTypes Type;
        public string CustomListId;
    }


    class ConfiguratedFile : InputFile
    {
        protected Dictionary<string, MixedList> MixedListTypes;
        protected List<DataNode[]> FileStructure;

        public ConfiguratedFile()
        {
            FileStructure = new List<DataNode[]>();
            MixedListTypes = new Dictionary<string, MixedList>();

            StreamReader s = File.OpenText("vartypes.in");

            //Load all the variable types
            string read = s.ReadLine();
            while (read != null)
            {
                string[] split = read.Split(new[] { ' ', '\t' });
                if(split.Length > 0)
                {
                    
                    var mixedList = new MixedList();

                    string name = split[0];
                    for (int i = 1; i < split.Length; ++i)
                    {
                        switch (split[i])
                        {
                            case "i":
                                mixedList.AddNewFieldType(DataTypes.Integer);
                                break;
                            case "d":
                                mixedList.AddNewFieldType(DataTypes.Double);
                                break;
                            case "s":
                                mixedList.AddNewFieldType(DataTypes.String);
                                break;
                        }
                        
                    }

                    MixedListTypes.Add(name, mixedList);
                }

                read = s.ReadLine();
            }

            s.Close();
        }

        protected void CreateConfig(string configFilename) 
		{
			StreamReader s = File.OpenText(configFilename);
            try
            {
	            //Read line
	            string read = s.ReadLine();
	            while (read != null)
	            {

		            //A line may have many data members so we have to split to get each one
		            var split = read.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);

		            //Create the data members for this line
		            var node = new DataNode[split.Length];
		            for (int i = 0; i < split.Length; ++i)
		            {
			            //Create the data member
			            node[i] = new DataNode();

			            //Split the data member to get its name (id) and type
			            string[] data = split[i].Split(new[] {'\\'});

			            //Data ID
			            node[i].Id = data[0];

			            //Data Type
			            switch (data[1])
			            {
				            case "i":
					            node[i].Type = DataTypes.Integer;
					            DataManager.Set(data[0], Int32.Parse(data[2]));
					            break;
				            case "d":
					            node[i].Type = DataTypes.Double;
					            DataManager.Set(data[0], Double.Parse(data[2]));
					            break;
				            case "li":
					            node[i].Type = DataTypes.IntegerList;
					            node[i].Dependent = data[2];
					            DataManager.Set(data[0], new List<string[]>());
					            break;
				            case "ld":
					            node[i].Type = DataTypes.DoubleList;
					            node[i].Dependent = data[2];
					            DataManager.Set(data[0], new List<string[]>());
					            break;
				            case "s":
					            node[i].Type = DataTypes.String;
					            DataManager.Set(data[0], data[2]);
					            break;
				            default:
					            if (MixedListTypes.ContainsKey(data[1]))
					            {
						            node[i].Type = DataTypes.Custom;
						            node[i].CustomListId = data[1];
						            node[i].Dependent = data[2];
						            DataManager.Set(data[0], new List<string[]>());
					            }
					            else
					            {
						            MessageBox.Show(Universal.Idioma(
                                        "WARNING: The variable type \"" + data[1] + "\" was not found. Is it defined in vartypes.in?", 
                                        "ADVERTENCIA: La variable de tipo  \"" + data[1] + "\" no se encontró. ¿Está definida en vartypes.in?"),
							            "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					            }
					            break;
			            }
		            }

		            //Add data member to structure
		            FileStructure.Add(node);

		            read = s.ReadLine();
	            }

	          }
            catch (Exception ex)
            {
	            MessageBox.Show(Universal.Idioma("ERROR 2304121128: error while reading file ", "ERROR 2304121128: error leyendo archivo ") + 
                    configFilename + ". " + ex.Message, "RiverFlow2D", MessageBoxButtons.OK , MessageBoxIcon.Error);
            }
            finally
            {
	            if (s != null)
		            s.Close();
            }

        }

        private void ProcessDependentVariables(StreamReader s, int i)
        {
	        //If variable on which it depends is 0 then skip
	        var dep = (int)DataManager.Get(FileStructure[i][0].Dependent);
	        if (dep == 0) return;

	        var list = new List<string[]>();

	        for (int j = 0; j < dep; ++j)
	        {
		        //Read line and split
		        string read = s.ReadLine();
		        string[] split = read.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

		        int cant = 0;
		        for (int k = 0; k < split.Length; ++k)
		        {
			        if (split[k] != " " && split[k] != "" && split[k] != "\t") ++cant;
		        }

		        var row = new string[cant];

		        //Create the proper array with the data from the line.
		        int count = 0;
		        for (int k = 0; k < split.Length; ++k)
		        {
			        if (split[k] == " " || split[k] == "" || split[k] == "\t") continue;

			        switch (FileStructure[i][0].Type)
			        {
				        case DataTypes.IntegerList:
					        row[count] = split[k];
					        break;
				        case DataTypes.DoubleList:
					        row[count] = Convert.ToDouble(split[k]).ToString("N2");
					        break;

				        case DataTypes.Custom:
					        switch (MixedListTypes[FileStructure[i][0].CustomListId].GetFieldType(k))
					        {
						        case DataTypes.Integer:
							        row[count] = split[k];
							        break;
						        case DataTypes.Double:
							        row[count] = Convert.ToDouble(split[k]).ToString("N2");
							        break;
						        case DataTypes.String:
							        row[count] = split[k];
							        break;
					        }
					        break;
			        }

			        ++count;
			        if (count == cant) break;
		        }

		        //Add data to list
		        list.Add(row);

	        }

	        //Add variable (list)
	        DataManager.Set(FileStructure[i][0].Id, list);
        }

        private void ProcessNormalVariables(StreamReader s, int i)
        {

		    string fullPath = ((FileStream) (s.BaseStream)).Name;
	   
            int j =0 ;
            //Read line
            if (!s.EndOfStream)
            {
                string read = s.ReadLine();
                try
                {
                    string[] split = read.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    //Set the data for each data member in the line
                    int cantElem = FileStructure[i].Length;

                    for (j = 0; j < cantElem; ++j)
                    {
                        if (FileStructure[i][j].Type == DataTypes.Integer)
                            if (j < split.Count())
                                DataManager.Set(FileStructure[i][j].Id, Convert.ToInt32(split[j]));
                            else
                                DataManager.Set(FileStructure[i][j].Id, 0);

                        else if (FileStructure[i][j].Type == DataTypes.Double)
                            if (j < split.Count())
                                DataManager.Set(FileStructure[i][j].Id, Convert.ToDouble(split[j]));
                            else
                                DataManager.Set(FileStructure[i][j].Id, Convert.ToDouble(split[j]));

                        else if (FileStructure[i][j].Type == DataTypes.String)
                            // TODO: This won't work if there are several variable strings in one line
                            if (j < split.Count())
                                DataManager.Set(FileStructure[i][j].Id, read);
                            else
                                DataManager.Set(FileStructure[i][j].Id, "");
                    }
                }
                catch (Exception ex)
                {
                    if (fullPath.ToUpper().EndsWith(".PLT"))
                    {
                        //do not write any message for now
                    }
                    else
                        if (FileStructure[i][j].Id != "EPSILON")
                        MessageBox.Show(Universal.Idioma("ERROR 2304121133: error while reading control tag '", "ERROR 2304121133: error leyendo control '") 
                            + FileStructure[i][j].Id + "'. " + ex.Message, "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public virtual void Load(string filename)
        {
            UpdateDependants();

            //Load all data into the data manager
            StreamReader s = File.OpenText(filename);

            try
            {
                int cantLines = FileStructure.Count;
                for (int i = 0; i < cantLines; ++i)
                {
                    //Check if data member is dependent
                    if (FileStructure[i][0].Dependent != null)
                    {
                        ProcessDependentVariables(s, i);
                    }
                    else
                    {
                        ProcessNormalVariables(s, i);
                    }
                }
            }
            catch (Exception ex)
            {
							MessageBox.Show(Universal.Idioma("ERROR 1205121043: error trying to open file ", "ERROR 1205121043: error abriendo archivo ") 
                                + filename + ". " + Environment.NewLine + ex.Message, "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
               s.Close();
            }
        }

        private void UpdateDependants()
        {

            for (int i = 0; i < FileStructure.Count; ++i)
            {
                for (int j = 0; j < FileStructure[i].Length; ++j)
                {
                    try
                    {
                        if (FileStructure[i][j].Dependent != null)
                        {
                            var val = (List<string[]>)DataManager.Get(FileStructure[i][j].Id);
                            DataManager.Set(FileStructure[i][j].Dependent, val.Count);
                        }
                    }
                    catch (Exception)
                    {
                        
                    }
                }
            }
        }


				//Save data to file
        public virtual void Save(string filename)
        {       
            TextWriter w = new StreamWriter(filename);
            try
            {
                //Some variables have to be updated with the size of list (or dependants)
                UpdateDependants();

                int cantLines = FileStructure.Count;
                for (int i = 0; i < cantLines; ++i)
                {
                    if (FileStructure[i][0].Dependent != null)
                    {
                        if (FileStructure[i][0].CustomListId == null)
                        {
                            SaveList(w, (List<string[]>)DataManager.Get(FileStructure[i][0].Id));
                        }
                        else
                        {
                            SaveMixedList(w, (List<string[]>)DataManager.Get(FileStructure[i][0].Id), 
															            MixedListTypes[FileStructure[i][0].CustomListId]);
                        }
                    }
                    else
                    {
                        int cantElem = FileStructure[i].Length;
                        string newLine = "";

                        for (int j = 0; j < cantElem; ++j)
                        {
                            //see if .DAT, .PLT, .SED, .AD, .MUD, .WIND, .OILP
                            if (ValFile.SaveRiverFLO2DdataVal)
                            {
                                if (filename.ToUpper().Contains(".DAT") || filename.ToUpper().Contains(".PLT") || filename.ToUpper().Contains(".SED")
																		|| filename.ToUpper().Contains(".AD") || filename.ToUpper().Contains(".MUD") || filename.ToUpper().Contains(".WIND")
																	  || filename.ToUpper().Contains(".OILP"))
                                {
                                    // store to later write RiverFLO2Ddata.VAL

                                    ValFile.RiverFLO2DdataVal[ValFile.ValIndex] = "" + FileStructure[i][j].Id;
                                    ValFile.ValIndex += 1;

                                    ValFile.RiverFLO2DdataVal[ValFile.ValIndex] = "" + DataManager.Get(FileStructure[i][j].Id);
                                    ValFile.ValIndex += 1;
                                }
                            }

                            // built newline to write filename
                            if (FileStructure[i][j].Type == DataTypes.Integer ||
                                FileStructure[i][j].Type == DataTypes.Double || FileStructure[i][j].Type == DataTypes.String)
                                newLine += DataManager.Get(FileStructure[i][j].Id);

                            if (j < cantElem - 1) newLine += " ";
                        }

                        w.WriteLine(newLine);
												
                    }
                }

            }
            catch (Exception ex)
            {
							MessageBox.Show(Universal.Idioma("ERROR 2399111836: error while saving file ", "ERROR 2399111836: error almacenando archivo ")
                                + filename + ". " + Environment.NewLine + ex.Message, "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
              w.Close();
            }
        }

        private void SaveMixedList(TextWriter w, List<string[]> list, MixedList listType)
        {
            for (int i = 0; i < list.Count; ++i)
            {
                string newLine = "";
                string[] line = list[i];
                for (int j = 0; j < line.Length && j < listType.NumberOfFields(); ++j)
                {
                    switch (listType.GetFieldType(j))
                    {
                        case DataTypes.Integer:
                            newLine += int.Parse(line[j]).ToString();
                            break;
                        case DataTypes.Double:
                            newLine += Double.Parse(line[j]).ToString();
                            break;
                        case DataTypes.String:
                            newLine += line[j];
                            break;
                    }

                    if (j < line.Length - 1) newLine += " ";
                }
                w.WriteLine(newLine);
            }
        }

        private void SaveList(TextWriter w, List<string[]> list)
        {
            for (int i = 0; i < list.Count; ++i)
            {
                string newLine = "";
                string[] line = list[i];
                for (int j = 0; j < line.Length; ++j)
                {
                    newLine += Double.Parse(line[j]).ToString();
                    if (j < line.Length - 1) newLine += " ";
                }
                w.WriteLine(newLine);
            }
        }

    }
}
