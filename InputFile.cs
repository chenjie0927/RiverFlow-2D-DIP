namespace Simulation_Options
{
  
    class InputFile
    {
      protected DataManager DataManager;

	    protected InputFile()
        {
            DataManager = DataManager.Instance;
        }

	    protected void AddVariable(string variableName, object variable)
        {
            DataManager.Set(variableName, variable);
        }

	    protected object GetVariable(string variableName)
        {
            return DataManager.Get(variableName);
        }

    }
}
