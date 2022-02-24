using System.Collections.ObjectModel;

namespace EasySaveSupervisor.model;

public sealed class Model
{
    private static readonly Model _instance = new();

    //Private variable
    private readonly ObservableCollection<Backup> _saveWorkList;

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

    public void SetBackups(IEnumerable<Backup> backups)
    {
        _saveWorkList.Clear();

        foreach (var backup in backups)
        {
            _saveWorkList.Add(backup);
        }
    }
}