using System.Collections.ObjectModel;
using System.Windows.Data;
using EasySaveSupervisor.model;
using EasySaveSupervisor.view.core;

namespace EasySaveSupervisor.viewmodel;

public class BackupsViewModel : ViewModelBase
{
    //PRIVATE VARIABLE
    //private List<Task> tasks;
    private readonly Model _model;
    private string _filterText;

    private readonly Client _client;
    
    public Backup SelectedBackup { get; set; }


    //CONSTRUCTOR
    public BackupsViewModel()
    {
        _model = Model.GetInstance();
        _client = Client.GetInstance();
    }

    public ObservableCollection<Backup> Backups => _model.GetBackups();

    //PUBLIC EVENT
    public event EventHandler<string> OnProgressUpdate;

    public void Filter(string filterText)
    {
        _filterText = filterText;

        var view = CollectionViewSource.GetDefaultView(Backups);
        view.Filter = item =>
        {
            if (item is Backup backup) return backup.Name.ToLower().Contains(_filterText.ToLower());

            return true;
        };
    }

    public void ExecuteAllBackups()
    {
        _client.ExecuteAllBackups();
    }

    public void GetAllBackups()
    {
        var backups = _client.GetAllBackups();
        _model.SetBackups(backups);
    }

    public void ExecuteSelectedBackup()
    {
        _client.ExecuteBackup(SelectedBackup);
    }

    public void ResumeSelectedBackup()
    {
        _client.ResumeBackup(SelectedBackup);
    }

    public void PauseSelectedBackup()
    {
        _client.PauseBackup(SelectedBackup);
    }

    public void CancelSelectedBackup()
    {
        _client.CancelBackup(SelectedBackup);
    }
}