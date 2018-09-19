using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;


namespace Simulation_Options
{
    public sealed class DataManager
    {
        //<Singleton>
        static DataManager _instance;
        static readonly object Padlock = new object();
        DataManager()
        {
            _data = new Dictionary<string, object>();
        }

        public static DataManager Instance
        {
            get
            {
                lock (Padlock)
                {
	                return _instance ?? (_instance = new DataManager());
                }
            }
        }

        public void Init()
        {
            _dataFile = new DataFile();
            _retFile = new RETFile(); 
            _sedimentFile = new SedimentFile();
            _pltFile = new PLTFile();
            _xsecsFile = new XSECSFile();
            _profilesFile = new PROFILESFile();
            _inflowFile = new INFLOWFile();
            _outflowFile = new OUTFLOWFile();
            _piersFile = new PIERSFile();
            _sourcesFile = new SOURCESFile();
            _culvertsFile = new CULVERTSFile();
            _obsFile = new OBSFile();
            _iflFile = new IFLFile();
            _weirsFile = new WEIRSFile();
            _irtFile = new IRTFile();
            _fedFile = new FEDFile();
            _plotFile = new PLOTFile();
            _adFile = new ADFile();
            _windFile = new WINDFile();
            _mudFlowFile = new MUDFLOWFile();
            _oilpFile = new OILPFile();
            _oilplusFile = new OILPLUSFile();
            _sedsPlusFile = new SEDPLUSFile();
            _solutesFile = new SOLUTESFile();
            _gatesFile = new GATESFile();
            _damBreachFile = new DAMBREACHFile();
            _bridgesFile = new BRIDGESFile();
            _linfFile = new LINFFile();
            _lrainFile = new LRAINFile();
            _windPlusFile = new WINDPLUSFile();
            _obcPlusFile = new OBCPFile();

	        Changed = false;
        }
        //<\Singleton>

        private readonly Dictionary<string, object> _data;

      //Files
        private RETFile _retFile;
        private DataFile _dataFile;
        private SedimentFile _sedimentFile;
        private PLTFile _pltFile;
        private XSECSFile _xsecsFile;
        private PROFILESFile _profilesFile;
        private INFLOWFile _inflowFile;
        private OUTFLOWFile _outflowFile;
        private PIERSFile _piersFile;
        private SOURCESFile _sourcesFile;
        private CULVERTSFile _culvertsFile;
        private OBSFile _obsFile;
        private WEIRSFile _weirsFile;
        private IFLFile _iflFile;
        private IRTFile _irtFile;
        private FEDFile _fedFile;
        private PLOTFile _plotFile;
        private ADFile _adFile;
        private WINDFile _windFile;
        private MUDFLOWFile _mudFlowFile;
        private OILPFile _oilpFile;
        private OILPLUSFile _oilplusFile;
        private SEDPLUSFile _sedsPlusFile;
        private SOLUTESFile _solutesFile;
        private GATESFile _gatesFile;
        private DAMBREACHFile _damBreachFile;
        private BRIDGESFile _bridgesFile;
        private LINFFile _linfFile;
	    private LRAINFile _lrainFile;
        private WINDPLUSFile _windPlusFile;
        private OBCPFile _obcPlusFile;


      //If data has changed
      public bool Changed;

	    public object Get(string nameOfVariable)
        {
            try
            {
                object retObj = _data[nameOfVariable];
                return retObj;
            }
            catch (Exception )
            {
                return null;
            }
        }

        public void Set(string nameOfVariable, object value)
        {
            Changed = true;
            _data[nameOfVariable] = value;
        }

        //.DAT Files
            public void SaveControlDataToFile(string filename)
            {
                _dataFile.Save(filename);

            }
            public void LoadControlDataFromFile(string filename)
            {
                _dataFile.Load(filename);
            }

        //.SED Files
            public void SaveSedimentToFile(string filename)
            {
                _sedimentFile.Save(filename);
            }
            public void LoadSedimentFromFile(string filename)
            {
                _sedimentFile.Load(filename);
            }

        //.PLOT Files
            public void SavePlotToFile(string filename)
            {
                _plotFile.Save(filename);
            }
            public void LoadPlotFromFile(string filename)
            {
                _plotFile.Load(filename);
            }

		//.AD Files
		    public void SaveAdToFile(string filename)
		    {
			    _adFile.Save(filename);
		    }
		    public void LoadAdFromFile(string filename)
		    {
			    _adFile.Load(filename);
		    }

		    //.WIND Files
		    public void SaveWindToFile(string filename)
		    {
			    _windFile.Save(filename);
		    }
		    public void LoadWindFromFile(string filename)
		    {
			    _windFile.Load(filename);
		    }

		//.WIND Plus Files
			public void SaveWindPlusToFile(string filename)
			{
				_windPlusFile.Save(filename);
			}
			public void LoadWindPlusFromFile(string filename)
			{
				_windPlusFile.Load(filename);
			}

		//.MUD Files
			public void SaveMudToFile(string filename)
			{
				_mudFlowFile.Save(filename);
			}
			public void LoadMudFromFile(string filename)
			{
				_mudFlowFile.Load(filename);
			}

		//.OILP Files
		    public void SaveOilpToFile(string filename)
		    {
			    _oilpFile.Save(filename);
		    }
		    public void LoadOilpFromFile(string filename)
		    {
			    _oilpFile.Load(filename);
		    }

		//.OILP Files (Plus)
		    public void SaveOilPlusToFile(string filename)
		    {
			    _oilplusFile.Save(filename);
		    }
		    public void LoadOilPlusFromFile(string filename)
		    {
			    _oilplusFile.Load(filename);
		    }

        //.RET Files
            public void SaveRainfallToFile(string filename)
            {
                _retFile.Save(filename);
            }
            public void LoadRainfallFromFile(string filename)
            {
                _retFile.Load(filename);
            }

		//.LRAIN Files
		    public void SaveLRainFallToFile(string filename)
		    {
			    _lrainFile.Save(filename);
		    }
		    public void LoadLRainFallFromFile(string filename)
		    {
			    _lrainFile.Load(filename);
		    }

        //.WEIRS Files
            public void SaveWeirsToFile(string filename)
            {
                _weirsFile.Save(filename);
            }
            public void LoadWeirsFromFile(string filename)
            {
                _weirsFile.Load(filename);
            }

        //.OBS Files
            public void SaveObservationPointsToFile(string filename)
            {
                _obsFile.Save(filename);
            }
            public void LoadObservationPointsFromFile(string filename)
            {
                _obsFile.Load(filename);
            }

        //.CULVERTS Files
            public void SaveCulvertsToFile(string filename)
            {
                _culvertsFile.Save(filename);
            }
            public void LoadCulvertsFromFile(string filename)
            {
                _culvertsFile.Load(filename);
            }

		//.SEDS Files
		    public void SaveSedsToFile(string filename)
		    {
			    _sedsPlusFile.Save(filename);
		    }
		    public void LoadSedsFromFile(string filename)
		    {
  		    _sedsPlusFile.Load(filename);
		    }

		//.SOLUTES Files
		    public void SaveSolutesToFile(string filename)
		    {
			    _solutesFile.Save(filename);
		    }
		    public void LoadSolutesFromFile(string filename)
		    {
			    _solutesFile.Load(filename);
		    }

		//.GATES Files
		    public void SaveGatesToFile(string filename)
		    {
			    _gatesFile.Save(filename);
		    }
		    public void LoadGatesFromFile(string filename)
		    {
			    _gatesFile.Load(filename);
		    }

        //.DAMBREACH Files
            public void SaveDamBreachToFile(string filename)
            {
              _damBreachFile.Save(filename);
            }
            public void LoadDamBreachFromFile(string filename)
            {
              _damBreachFile.Load(filename);
            }

        //.LINF Files
            public void SaveInfiltrationToFile(string filename)
		    {
			    _linfFile.Save(filename);
		    }
		    public void LoadInfiltrationFromFile(string filename)
		    {
			    _linfFile.Load(filename);
		    }

		//.BRIDGES Files
		    public void SaveBridgesToFile(string filename)
		    {
			    _bridgesFile.Save(filename);
		    }
		    public void LoadBridgesFromFile(string filename)
		    {
			    _bridgesFile.Load(filename);
		    }

        //.PIERS Files
            public void SavePiersToFile(string filename)
            {
                _piersFile.Save(filename);
            }
            public void LoadPiersFromFile(string filename)
            {
                _piersFile.Load(filename);
            }

        //.SOURCES Files
            public void SaveSourcesToFile(string filename)
            {
                _sourcesFile.Save(filename);
            }
            public void LoadSourcesFromFile(string filename)
            {
                _sourcesFile.Load(filename);
            }

            //public void loadSOURCESGroupSeries(int node)
            //{
            //    _sourcesFile.LoadSOURCESSeries(node);
            //}

        //.PLT Files
            public void SaveGraphicOptionsToFile(string filename)
            {
                _pltFile.Save(filename);
            }
            public void LoadGraphicOptionsFromFile(string filename)
            {
                _pltFile.Load(filename);
            }

        //.IFL Files
            public void SaveOpenBoundaryConditionsToFile(string filename)
            {
                _iflFile.Save(filename);
            }
            public void LoadOpenBoundaryConditionsFromFile(string filename)
            {
                _iflFile.Load(filename);
            }

		//.OBCP Files
		    public void SaveOpenBoundaryPlusToFile(string filename)
		    {
			    _obcPlusFile.Save(filename);
		    }
		    public void LoadOpenBoundaryPlusFromFile(string filename)
		    {
			    _obcPlusFile.Load(filename);
		    }
		    public void loadOBCGroupNodes(int node)
		    {
			    _obcPlusFile.LoadOBCPNodes(node);
		    }
		    public void loadOBCGroupSeries(int node)
		    {
			    _obcPlusFile.LoadOBCSeries(node);
		    }

        //.XSECS Files
            public void SaveCrossSectionsToFile(string filename)
            {
                _xsecsFile.Save(filename);
            }
            public void LoadCrossSectionsFromFile(string filename)
            {
                _xsecsFile.Load(filename);
            }

        //.PROFILES Files
            public void SaveProfileCutsToFile(string filename)
            {
                _profilesFile.Save(filename);
            }
            public void LoadProfileCutsFromFile(string filename)
            {
                _profilesFile.Load(filename);
            }

        //.IRT Files
            public void SaveInternalRatingTablesToFile(string filename)
            {
                _irtFile.Save(filename);
            }
            public void LoadInternalRatingTablesFromFile(string filename)
            {
                _irtFile.Load(filename);
            }

        //Inflow Boundary Files
            public void SaveInflowToFile(string filename)
            {
                _inflowFile.Save(filename);
            }
            public void LoadInflowFromFile(string filename)
            {
                _inflowFile.Load(filename);
            }

        //Outflow Boundary Files
            public void SaveOutflowToFile(string filename)
            {
                _outflowFile.Save(filename);
            }
            public void LoadOutflowFromFile(string filename)
            {
                _outflowFile.Load(filename);
            }


        public string[] ReadSourceSfileNames(string filename)
        {
	        string[] fileNames = _sourcesFile.ReadFileNames(filename);
	        return fileNames;
        }

	    public string[] ReadCoulvertSfileNames(string filename)
	    {
		    string[] fileNames = _culvertsFile.ReadFileNames(filename);
		    return fileNames;
	    }

	    public string[] ReadIrTfileNames(string filename)
	    {
		    string[] fileNames = _irtFile.ReadFileNames(filename);
		    return fileNames;
	    }

	    public string[] ReadIfLfileNames(string filename)
	    {
		    string[] fileNames = _iflFile.ReadFileNames(filename);
		    return fileNames;
	    }

	    public string[] ReadFeDfileNames(string filename)
	    {
		    string[] fileNames = _fedFile.ReadFileNames(filename);
		    return fileNames;
	    }

    }
}
