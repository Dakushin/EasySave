using System.IO;

namespace EasySave.model;

internal class Model
{
    //Private variable
    private readonly string _logPath;
    private readonly string _saveStatePath;
    private readonly List<SaveWork> _saveWorkList;
    private bool _workInProgress;
    
    //CONSTRUCTOR
    public Model()
    {
        if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EasySave"))
        {
            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                      "\\EasySave");
        }
        _logPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EasySave\\log.json";
        _saveStatePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EasySave\\state.json";
        _saveWorkList = new List<SaveWork>(5);
        _workInProgress = false;
    }

    //GETTER AND SETTER
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

    //find Savework by name
    public SaveWork FindbyName(string name)
    {
        foreach (var sv in _saveWorkList)
            if (sv.GetName() == name)
                return sv;
        return null;
    }
}