using System.Collections.ObjectModel;
using System.IO;
using EasySave.model.backupStrategies;

namespace EasySave.model;

public sealed class Model
{
    private static readonly Model _instance = new();
    
    //Private variable
    private readonly string _logPath;
    private readonly string _saveStatePath;
    private readonly ObservableCollection<Backup> _saveWorkList;
    private bool _workInProgress;
    private List<string> listProcessToCheck;
    private List<string> listExtentionToCheck;
    private FileFormat _logFileFormat;

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
        _saveWorkList = new ObservableCollection<Backup>();
        _workInProgress = false;
        _logFileFormat = new Json();
        listProcessToCheck = new List<string>() { "Calculator", "word", "notepad", "WINWORD", "chrome" };
        listExtentionToCheck = new List<string>() { ".png", ".jpeg", ".jpg" };

        _saveWorkList.Add(new Backup("test", @"C:\Users\sacha\Desktop\test\1", @"C:\Users\sacha\Desktop\test\2", new Complete()));
        _saveWorkList.Add(new Backup("bonjour_monde", @"C:\Users\sacha\Desktop\test\1", @"C:\Users\sacha\Desktop\test\2", new Complete()));
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

    public ObservableCollection<Backup> GetBackupList()
    {
        return _saveWorkList;
    }

    //find Savework by name
    public Backup FindbyName(string name)
    {
        foreach (var sv in _saveWorkList)
            if (sv.Name == name)
                return sv;
        return null;
    }

    public FileFormat GetLogFileFormat()
    {
        return _logFileFormat;
    }

    public void SetLogFileFormat(FileFormat x)
    {
        _logFileFormat = x;
    }

    public List<string> GetListProcessToCheck()
    {
        return listProcessToCheck;
    }

    public List<string> GetListExtentionToCheck()
    {
        return listExtentionToCheck;
    }
}