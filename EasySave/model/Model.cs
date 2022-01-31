namespace EasySave.model;

internal class Model
{
    private readonly List<string> _errorMessage;
    private readonly string _logPath;
    private readonly string _saveStatePath;
    private readonly List<SaveWork> _saveWorkList;
    private bool _workInProgress;

    public Model()
    {
        _logPath = Path.GetFullPath(@"..\..\..\log.json");
        _saveStatePath = Path.GetFullPath(@"..\..\..\state.json");
        ;
        _saveWorkList = new List<SaveWork>(5);
        _errorMessage = new List<string>
        {
            "Error, you have already 5 savework, please delete one and retry",
            "Error, Does not find the name of the work you want to delete ",
            "Error, Savework with this name already exist",
            "Error, You can't rename you Savework with name you already given"
        };
        _workInProgress = false;
    }

    public string GetLogPath()
    {
        return _logPath;
    }

    public string GetSaveStatePath()
    {
        return _saveStatePath;
    }

    public List<SaveWork> GetSaveWorkList()
    {
        return _saveWorkList;
    }

    public List<string> GetErrorList()
    {
        return _errorMessage;
    }

    public SaveWork FindbyName(string name)
    {
        foreach (var sv in _saveWorkList)
            if (sv.GetName() == name)
                return sv;
        return null;
    }
}