namespace EasySave.model;

internal class Model
{
    private readonly string _logPath;
    private readonly string _saveStatePath;
    private readonly List<SaveWork> _saveWorkList;
    private bool _workInProgress;

    public Model()
    {
        _logPath = Path.GetFullPath(@"..\..\..\log.json");
        _saveStatePath = Path.GetFullPath(@"..\..\..\state.json");
        _saveWorkList = new List<SaveWork>(5);
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

    public SaveWork FindbyName(string name)
    {
        foreach (var sv in _saveWorkList)
            if (sv.GetName() == name)
                return sv;
        return null;
    }
}