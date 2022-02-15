using System.Collections.ObjectModel;
using System.IO;

namespace EasySave.model;

public sealed class Model
{
    private static readonly Model _instance = new();
    
    //Private variable
    private readonly string _logPath;
    private readonly string _saveStatePath;
    private readonly ObservableCollection<SaveWork> _saveWorkList;
    private bool _workInProgress;
    
    //CONSTRUCTOR
    private Model()
    {
        if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EasySave"))
        {
            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                      "\\EasySave");
        }
        _logPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EasySave\\log.json";
        _saveStatePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EasySave\\state.json";
        _saveWorkList = new ObservableCollection<SaveWork>();
        _saveWorkList.Add(new SaveWork("EasySave backup", @"C:\Program Files (x86)\EasySave\", @"C:\Backup\EasySave\", SaveType.Differential));
        _saveWorkList.Add(new SaveWork("Personal photos", @"C:\Users\John\Pictures\", @"C:\Backup\Photos\", SaveType.Differential));
        _saveWorkList.Add(new SaveWork("Professional work", @"C:\Users\sacha\Desktop\test", @"C:\Backup\Work\", SaveType.Complete));
        _workInProgress = false;
    }
    
    public static Model GetInstance()
    {
        return _instance;
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

    public ObservableCollection<SaveWork> GetSaveWorkList()
    {
        return _saveWorkList;
    }

    //find Savework by name
    public SaveWork FindbyName(string name)
    {
        foreach (var sv in _saveWorkList)
            if (sv.Name == name)
                return sv;
        return null;
    }
}