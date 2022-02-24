using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using EasySaveSupervisor.model;
using EasySaveSupervisor.model.backupStrategies;
using EasySaveSupervisor.properties;
using EasySaveSupervisor.view.core;

namespace EasySaveSupervisor.viewmodel;

public class BackupsViewModel : ViewModelBase
{
    //PRIVATE VARIABLE
    //private List<Task> tasks;
    private readonly Model _model;
    private string _filterText;
    private bool alreadyLaunch;

    private bool ProcessDetect;

    //CONSTRUCTOR
    public BackupsViewModel()
    {
        _model = Model.GetInstance();
    }

    public ObservableCollection<Backup> Backups => _model.GetBackupList();

    public Backup SelectedBackup { get; set; }

    //PUBLIC EVENT
    public event EventHandler<string> OnProgressUpdate;

    public void ExecuteSelectedBackup()
    {
        ExecuteBackup(SelectedBackup);
    }

    public void DeleteSelectedBackup()
    {
        DeleteBackup(SelectedBackup.Name);
    }

    public void ResumeSelectedBackup()
    {
        ResumeBackup(SelectedBackup);
    }

    public void ResumeBackup(Backup backup)
    {
        ProcessDetect = CheckIfWorkProcessIsOpen(Model.GetInstance().GetListProcessToCheck()) == string.Empty
            ? false
            : true;
        if (!ProcessDetect) backup.BackupStrategy.Resume();
        ThreadCheckingWorkingSoftware();
    }

    public void PauseSelectedBackup()
    {
        PauseBackup(SelectedBackup);
    }

    public void CancelSelectedBackup()
    {
        CancelBackup(SelectedBackup);
    }

    public void PauseBackup(Backup backup)
    {
        backup.BackupStrategy.Pause();
    }

    public void CancelBackup(Backup backup)
    {
        backup.BackupStrategy.Cancel();
    }

    public void CreateBackup(string name, string sourcePath, string targetPath, BackupStrategy backupStrategy,
        bool isEncrypted)
    {
        if (_model.GetBackupList().Count < 5) //check if we have more than 5 backups
        {
            if (_model.FindbyName(name) == null) //check if it already exists
            {
                if (Directory.Exists(sourcePath) || File.Exists(sourcePath)) // check if source path is good
                {
                    targetPath = Path.GetFullPath(targetPath);
                    _model.GetBackupList().Add(new Backup(name, sourcePath, targetPath, backupStrategy, isEncrypted));
                    NotifySuccess(Resources.Backup_Created);
                }
                else
                {
                    NotifyError(Resources.Error_Wrong_SourcePath);
                }
            }
            else
            {
                NotifyError(Resources.Error_Backup_Already_Exists);
            }
        }
        else
        {
            NotifyError(Resources.Error_Too_Many_Backups);
        }
    }

    public void DeleteBackup(string name) //function to delete a Savework by name
    {
        var sv = _model.FindbyName(name);
        if (sv != null)
        {
            _model.GetBackupList().Remove(sv);
            NotifySuccess(Resources.Success);
        }
        else
        {
            NotifyError(Resources.Error_Backup_Not_Found);
        }
    }

    public async void ExecuteBackup(Backup backup)
    {
        var soft = CheckIfWorkProcessIsOpen(Model.GetInstance().GetListProcessToCheck());
        if (soft == string.Empty)
        {
            ThreadCheckingWorkingSoftware();
            backup.IsExecute = true;
            var success = await Task.Run(backup.Execute);

            if (success)
                NotifySuccess($"{backup.Name} {Resources.Success_Execution}");
            else
                NotifyError($"{backup.Name} {Resources.Cancelled}");
            backup.IsExecute = false;
        }
        else
        {
            NotifyError(Resources.Error_WorkingProcess + $" {soft}");
        }
    }

    private async void ThreadCheckingWorkingSoftware()
    {
        var software = string.Empty;
        if (!alreadyLaunch)
        {
            alreadyLaunch = true;
            software = await Task.Run(CheckProcesses);
            if (software != null && !ProcessDetect)
            {
                if (SelectedBackup != null)
                {
                    NotifyError(Resources.Error_WorkingProcess + $" {software}");
                    PauseAllBackup();
                }

                ProcessDetect = true;
                alreadyLaunch = false;
            }
        }
    }

    public void PauseAllBackup()
    {
        foreach (var backup in Model.GetInstance().GetBackupList())
            if (backup.IsExecute)
                backup.BackupStrategy.Pause();
    }

    private void GetAllFileFromDirectory(string[] directories, List<string> files) //return all file in a directory
    {
        foreach (var directory in directories)
        {
            var list = Directory.GetDirectories(directory);
            if (list.Length > 0) GetAllFileFromDirectory(list, files);

            files.AddRange(Directory.EnumerateFiles(directory));
        }
    }

    public void ExecAllBackup() //Execute all savework
    {
        if (_model.GetBackupList().Count > 0)
            foreach (var sw in _model.GetBackupList())
                ExecuteBackup(sw);
        else
            NotifyInfo(Resources.Info_No_Backup);
    }

    public void ChangeLanguage(Language language) //Function to change language
    {
        CultureInfo.CurrentUICulture = language switch
        {
            Language.English => CultureInfo.GetCultureInfo("en"),
            Language.French => CultureInfo.GetCultureInfo("fr"),
            _ => CultureInfo.CurrentUICulture
        };
        NotifySuccess(Resources.Success);
    }

    public void ChangeFileFormat(FileFormat fileFormat)
    {
        _model.SetLogFileFormat(fileFormat);
        NotifySuccess(Resources.Success);
    }


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

    public static string CheckProcesses()
    {
        while (CheckIfWorkProcessIsOpen(Model.GetInstance().GetListProcessToCheck()) == string.Empty)
            Thread.Sleep(1000);
        return CheckIfWorkProcessIsOpen(Model.GetInstance().GetListProcessToCheck());
    }

    public static string CheckIfClose(string process)
    {
        return CheckIfWorkProcessIsOpen(Model.GetInstance().GetListProcessToCheck());
    }

    private static string
        CheckIfWorkProcessIsOpen(List<string> listOfProcessToCheck) //Function for check if job Process is on
    {
        foreach (var ProcessToCheck in listOfProcessToCheck)
        {
            var processes = Process.GetProcessesByName(ProcessToCheck);
            if (processes.Length > 0) return ProcessToCheck;
        }

        return string.Empty;
    }
}