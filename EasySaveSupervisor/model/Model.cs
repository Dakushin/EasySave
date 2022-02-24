using System.Collections.ObjectModel;
using System.IO;
using EasySaveSupervisor.model.backupStrategies;

namespace EasySaveSupervisor.model;

public sealed class Model
{
    private static readonly Model _instance = new();

    //Private variable
    private readonly string _logPath;
    private readonly string _saveStatePath;
    private readonly ObservableCollection<Backup> _saveWorkList;
    private FileFormat _logFileFormat;
    private readonly List<string> _listExtentionToCheck;
    private readonly List<string> _listProcessToCheck;
    private readonly ObservableCollection<string> _listProrityExtension;

    //CONSTRUCTOR
    private Model()
    {
        if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EasySave"))
            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                      "\\EasySave");
        _logPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EasySave\\log-" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year;
        _saveStatePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                         "\\EasySave\\state.json";
        _saveWorkList = new ObservableCollection<Backup>();
        _logFileFormat = new Json();
        _listProcessToCheck = new List<string> {"Calculator", "word", "notepad", "WINWORD", "chrome"};
        _listExtentionToCheck = new List<string> {".png", ".jpeg", ".jpg"};
        _listProrityExtension = new ObservableCollection<string> { ".pdf", ".txt" };

        _saveWorkList.Add(new Backup(
            "test",
            @"C:\Users\sacha\Desktop\test\1",
            @"C:\Users\sacha\Desktop\test\2",
            new Complete())
        );

        TryRecupFromSaveStatePath();
    }

    public void TryRecupFromSaveStatePath() //Function to fetch savework unfinish from the savestate file
    {
        if (!File.Exists(GetSaveStatePath())) return;

        FileFormat fileFormat = new Json();
        List<BackupState> saveStates;


        saveStates = fileFormat.UnSerialize<BackupState>(GetSaveStatePath());
        foreach (var sv in saveStates)
            if (sv.GetNbFilesLeftToDo() > 0)
            {
                var listDirectorySource = new List<string>(sv.SourceFilePath.Split(Path.DirectorySeparatorChar));
                var listDirectoryTarget = new List<string>(sv.TargetFilePath.Split(Path.DirectorySeparatorChar));
                var sameDirectory = false;
                while (!sameDirectory && listDirectorySource.Any() &&
                       listDirectoryTarget.Any()) //Loop to fetch the original directory from all path
                    if (listDirectorySource.Last() == listDirectoryTarget.Last())
                    {
                        listDirectorySource.Remove(listDirectorySource.Last());
                        listDirectoryTarget.Remove(listDirectoryTarget.Last());
                    }
                    else
                    {
                        sameDirectory = true;
                    }

                var sourcePath = string.Join(Path.DirectorySeparatorChar, listDirectorySource);
                var targetPath = string.Join(Path.DirectorySeparatorChar, listDirectoryTarget);
                var strategy = new Differential();
                strategy.SetFileLeftToDo(sv.NbFilesLeftToDo);
                _saveWorkList.Add(new Backup(sv.GetName(), sourcePath, targetPath, new Differential()));
            }
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
    public Backup? FindbyName(string name)
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
        return _listProcessToCheck;
    }

    public List<string> GetListExtentionToCheck()
    {
        return _listExtentionToCheck;
    }

    public ObservableCollection<string> GetListPriorityExtension()
    {
        return _listProrityExtension;
    }
}