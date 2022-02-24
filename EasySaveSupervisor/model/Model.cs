using System.Collections.ObjectModel;

namespace EasySaveSupervisor.model;

public sealed class Model
{
    private static readonly Model _instance = new();

    //Private variable
    private ObservableCollection<Backup> _saveWorkList;

    //CONSTRUCTOR
    private Model()
    {
        _saveWorkList = new ObservableCollection<Backup>();
    }

    public static Model GetInstance()
    {
        return _instance;
    }

    public ObservableCollection<Backup> GetBackups()
    {
        return _saveWorkList;
    }

    public void SetBackups(Backup[] backups)
    {
        // spaghetti code but no time :(
        var backupsToRemove = new List<Backup>(_saveWorkList.Count);
        backupsToRemove.AddRange(_saveWorkList.Where(clientBackup => !backups.Contains(clientBackup)));

        foreach (var backup in backupsToRemove)
        {
            _saveWorkList.Remove(backup);
        }
        
        foreach (var serverBackup in backups)
        {
            if (_saveWorkList.Contains(serverBackup))
            {
                var index = _saveWorkList.IndexOf(serverBackup);
                _saveWorkList[index].Progression = serverBackup.Progression;
            }
            else
            {
                _saveWorkList.Add(serverBackup);
            }
        }
    }
}